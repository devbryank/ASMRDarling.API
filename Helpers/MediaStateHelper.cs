using Amazon;
using Amazon.Runtime;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ASMRDarling.API.Models;
using ASMRDarling.API.Templates;

namespace ASMRDarling.API.Helpers
{
    /// <summary>
    /// aws dynamo db management helper
    /// </summary>
    class MediaStateHelper
    {
        ILambdaLogger logger;
        DynamoDBContext Context { get; set; }
        AmazonDynamoDBClient Client { get; set; }

        // db credentials
        readonly string accessKey = "";
        readonly string secretKey = "";

        // db info
        static readonly string hashKey = "UserId";
        static readonly string mediaStatesTable = "MediaStates";


        public MediaStateHelper(ILambdaLogger logger)
        {
            logger.LogLine($"[MediaStateHelper.MediaStateHelper()] Instantiating media state helper");
            this.logger = logger;
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            Client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
        }


        public async Task VerifyTable()
        {
            logger.LogLine($"[MediaStateHelper.VeryfyTable()] Verifying table");
            await VerifyTable(mediaStatesTable);
        }


        // verify and create table if not exist
        public async Task VerifyTable(string tableName)
        {
            var tableResponse = await Client.ListTablesAsync();

            if (!tableResponse.TableNames.Contains(tableName) && !CreateTable(tableName).Result)
            {
                logger.LogLine($"[MediaStateHelper.VeryfyTable()] Table veryfication failed");
            }

            logger.LogLine($"[MediaStateHelper.VeryfyTable()] Table veryfication success");
            Context = new DynamoDBContext(Client);
        }


        // create table
        private async Task<bool> CreateTable(string tableName)
        {
            logger.LogLine($"[MediaStateHelper.CreateTable()] Table does not exist, creating {tableName} table");

            await Client.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 3, WriteCapacityUnits = 1 },
                KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement{ AttributeName = hashKey, KeyType = KeyType.HASH }
                    },
                AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition { AttributeName = hashKey, AttributeType=ScalarAttributeType.S }
                    }
            });

            bool isTableAvailable = false;
            int waitLimit = 10;
            int waitCount = 0;

            while (!isTableAvailable)
            {
                Thread.Sleep(5000);

                var tableStatus = await Client.DescribeTableAsync(tableName);
                isTableAvailable = tableStatus.Table.TableStatus == "ACTIVE";
                waitCount++;

                if (waitLimit == waitCount)
                    return false;
            }

            logger.LogLine($"[MediaStateHelper.CreateTable()] Table {tableName} created");
            return true;
        }


        // save media state
        public async Task SaveMediaState(MediaState state)
        {
            logger.LogLine($"[MediaStateHelper.SaveMediaState()] Saving media state: {state}");
            await Context.SaveAsync(state);
        }


        // get media state
        public async Task<MediaState> GetMediaState(string userId)
        {
            logger.LogLine($"[MediaStateHelper.GetMediaState()] Getting media state for user id: {userId}");
            List<ScanCondition> conditions = new List<ScanCondition>
            {
                new ScanCondition("UserId", ScanOperator.Equal, userId)
            };

            var allDocs = await Context.ScanAsync<MediaState>(conditions).GetRemainingAsync();
            if (allDocs != null && allDocs.Count != 0)
            {
                return allDocs.FirstOrDefault();
            }

            var initialState = new MediaState() { UserId = userId };
            initialState.State = MediaConstants.SetDefaultState();

            logger.LogLine($"[MediaStateHelper.GetMediaState()] State acquired: {initialState}");
            return initialState;
        }
    }
}

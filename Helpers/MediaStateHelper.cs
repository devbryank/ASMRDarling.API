using Amazon;
using Amazon.Runtime;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sonnar.Core;
using ASMRDarling.API.Models;
using ASMRDarling.API.Constants;

namespace ASMRDarling.API.Helpers
{
    class MediaStateHelper
    {
        DynamoDBContext Context { get; set; }
        AmazonDynamoDBClient Client { get; set; }


        // initialize database helper
        public MediaStateHelper()
        {
            Core.Logger.Write($"[MediaStateHelper.MediaStateHelper()] Instantiating media state helper");
            var credentials = new BasicAWSCredentials(DbConstants.DbAccessKey, DbConstants.DbSecretKey);
            Client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
        }


        public async Task VerifyTable()
        {
            Core.Logger.Write($"[MediaStateHelper.VeryfyTable()] Table {DbConstants.MediaStatesTableName} verification started");
            await VerifyTable(DbConstants.MediaStatesTableName);
        }


        // verify and create table if not exist
        public async Task VerifyTable(string tableName)
        {
            Core.Logger.Write($"[MediaStateHelper.VeryfyTable()] Table {DbConstants.MediaStatesTableName} verification in progress");
            var tableResponse = await Client.ListTablesAsync();

            if (!tableResponse.TableNames.Contains(tableName) && !CreateTable(tableName).Result)
            {
                Core.Logger.Write($"[MediaStateHelper.VeryfyTable()] Table {tableName} verification failed");
                throw new Exception("Could not find or create table: " + tableName);
            }

            Core.Logger.Write($"[MediaStateHelper.VeryfyTable()] Table {tableName} verification success");
            Context = new DynamoDBContext(Client);
        }


        // create table if it does not exist
        private async Task<bool> CreateTable(string tableName)
        {
            Core.Logger.Write($"[MediaStateHelper.CreateTable()] Table does not exist, creating {tableName} table");

            await Client.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 3, WriteCapacityUnits = 1 },
                KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement{ AttributeName = DbConstants.HashKey, KeyType = KeyType.HASH }
                    },
                AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition { AttributeName = DbConstants.HashKey, AttributeType=ScalarAttributeType.S }
                    }
            });

            int waitLimit = 10;
            int waitCount = 0;
            bool isTableAvailable = false;

            // check if the table has been created
            while (!isTableAvailable)
            {
                Thread.Sleep(5000);

                var tableStatus = await Client.DescribeTableAsync(tableName);
                isTableAvailable = tableStatus.Table.TableStatus == "ACTIVE";
                waitCount++;

                if (waitLimit == waitCount)
                    return false;
            }

            Core.Logger.Write($"[MediaStateHelper.CreateTable()] Table {tableName} has been created");
            return true;
        }


        // save media state
        public async Task SaveMediaState(MediaState state)
        {
            Core.Logger.Write($"[MediaStateHelper.SaveMediaState()] Saving user's media state to {DbConstants.MediaStatesTableName}");
            Core.Logger.Write($"[MediaStateHelper.SaveMediaState()] User's media state details: {JsonConvert.SerializeObject(state)}");
            await Context.SaveAsync(state);
        }


        // get media state
        public async Task<MediaState> GetMediaState(string userId)
        {
            Core.Logger.Write($"[MediaStateHelper.GetMediaState()] Acquiring user's media state for requested user");
            Core.Logger.Write($"[MediaStateHelper.GetMediaState()] User id: {userId}");

            List<ScanCondition> conditions = new List<ScanCondition>
            {
                new ScanCondition(DbConstants.HashKey, ScanOperator.Equal, userId)
            };

            // get user's most recent media state from database if exist
            var allDocs = await Context.ScanAsync<MediaState>(conditions).GetRemainingAsync();
            if (allDocs != null && allDocs.Count != 0)
            {
                Core.Logger.Write($"[MediaStateHelper.GetMediaState()] User's media state details: {JsonConvert.SerializeObject(allDocs)}");
                return allDocs.FirstOrDefault();
            }

            // initialize user's media state if database is empty
            var initialState = new MediaState() { UserId = userId };
            initialState.State = MediaStateConstants.SetDefaultState();

            Core.Logger.Write($"[MediaStateHelper.GetMediaState()] User's media state initialized");
            Core.Logger.Write($"[MediaStateHelper.GetMediaState()] Initialized media details: {JsonConvert.SerializeObject(initialState)}");

            return initialState;
        }
    }
}

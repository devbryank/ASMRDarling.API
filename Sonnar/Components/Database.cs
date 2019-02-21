using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

using Amazon;
using Amazon.Runtime;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

using Sonnar.Models;

namespace Sonnar.Components
{
    class Database
    {
        protected string UserId { get; private set; }
        protected State UserState { get; private set; }
        protected DynamoDBContext DbContext { get; private set; }
        protected AmazonDynamoDBClient Client { get; private set; }


        public Database(string userId)
        {
            Core.Logger.Write("Database.Database()", "Connecting to the database");
            UserId = userId;
            BasicAWSCredentials credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("DbAccessKey"), Environment.GetEnvironmentVariable("DbSecretKey"));
            Client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
        }


        public async Task VerifyTable()
        {
            Core.Logger.Write("Database.VerifyTable()", $"Table {SkillSettings.TableName} verification started");
            await VerifyTable(SkillSettings.TableName);
        }


        public async Task VerifyTable(string tableName)
        {
            var tableResponse = await Client.ListTablesAsync();

            if (!tableResponse.TableNames.Contains(tableName) && !CreateTable(tableName).Result)
            {
                Core.Logger.Write("Database.VerifyTable()", $"Table {tableName} verification failed");
                throw new Exception("[Database.VerifyTable()] Could not find or create table: " + tableName);
            }

            Core.Logger.Write("Database.VerifyTable()", $"Table {tableName} verification completed");
            DbContext = new DynamoDBContext(Client);
        }


        private async Task<bool> CreateTable(string tableName)
        {
            Core.Logger.Write("Database.CreateTable()", $"Table does not exist, creating {tableName} table");

            await Client.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 3, WriteCapacityUnits = 1 },
                KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement{ AttributeName = SkillSettings.HashKey, KeyType = KeyType.HASH }
                    },
                AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition { AttributeName = SkillSettings.HashKey, AttributeType=ScalarAttributeType.S }
                    }
            });

            int waitLimit = 10;
            int waitCount = 0;
            bool isTableAvailable = false;

            while (!isTableAvailable)
            {
                Thread.Sleep(5000);

                var tableStatus = await Client.DescribeTableAsync(tableName);
                isTableAvailable = tableStatus.Table.TableStatus == "ACTIVE";
                waitCount++;

                if (waitLimit == waitCount)
                    return false;
            }

            Core.Logger.Write("Database.CreateTable()", $"Table {tableName} has been created");
            return true;
        }


        public async Task<State> GetState()
        {
            Core.Logger.Write("Database.GetState()", "Acquiring user state for the requested user");
            Core.Logger.Write("Database.GetState()", $"User id: {UserId}");

            List<ScanCondition> conditions = new List<ScanCondition>
            {
                new ScanCondition(SkillSettings.HashKey, ScanOperator.Equal, UserId)
            };

            var allDocs = await DbContext.ScanAsync<State>(conditions).GetRemainingAsync();
            if (allDocs != null && allDocs.Count != 0)
            {
                UserState = allDocs.FirstOrDefault();
                Core.Logger.Write("Database.GetState()", $"User state details: {JsonConvert.SerializeObject(allDocs)}");
                return UserState;
            }

            UserState = new State() { UserId = UserId };
            UserState.UserState = SetDefaultState();

            Core.Logger.Write("Database.GetState()", "User state initialized");
            Core.Logger.Write("Database.GetState()", $"Initialized state details: {JsonConvert.SerializeObject(UserState)}");

            return UserState;
        }


        public async Task SaveState()
        {
            Core.Logger.Write("Database.SaveState()", "Saving user state");
            Core.Logger.Write("Database.SaveState()", $"User state details: {JsonConvert.SerializeObject(UserState)}");
            await DbContext.SaveAsync(UserState);
        }


        private StateMap SetDefaultState()
        {
            StateMap map = new StateMap()
            {
                Index = 1,
                Loop = true,
                Shuffle = false,
                Token = null,
                EnqueuedToken = null,
                OffsetInMS = 0,
                PlaybackFinished = false,
                PlaybackIndexChanged = false,
                PlayOrder = new List<int>(),
                NumTimesPlayed = 0,
                State = "MENU_MODE",
            };

            return map;
        }
    }
}

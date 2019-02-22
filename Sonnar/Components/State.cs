using Amazon.DynamoDBv2.DataModel;

using Sonnar.Models;

namespace Sonnar.Components
{
    [DynamoDBTable(SkillSetting.TableName)]
    class State
    {
        [DynamoDBHashKey]
        public string UserId { get; set; }
        public StateMap UserState { get; set; }
    }
}

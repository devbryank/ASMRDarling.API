using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace ASMRDarling.API.Models
{
    [DynamoDBTable("MediaStates")]
    class MediaState
    {
        [DynamoDBHashKey]
        public string UserId { get; set; }
        public StateMap State { get; set; }
    }

    public class StateMap
    {
        public int Index { get; set; }
        public bool Loop { get; set; }
        public bool Shuffle { get; set; }
        public string Token { get; set; }
        public string EnqueuedToken { get; set; }
        public int OffsetInMS { get; set; }
        public bool PlaybackFinished { get; set; }
        public bool PlaybackIndexChanged { get; set; }
        public List<int> PlayOrder { get; set; }
        public string State { get; set; }
    }
}

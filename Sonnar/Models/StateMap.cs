using System.Collections.Generic;

namespace Sonnar.Models
{
    class StateMap
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
        public int NumTimesPlayed { get; set; }
        public int NumReprompt { get; set; }
        public string Stage { get; set; }
    }
}

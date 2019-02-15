using ASMRDarling.API.Models;
using System.Collections.Generic;

namespace ASMRDarling.API.Templates
{
    static class MediaConstants
    {
        public static string AppId = "";
        private static string dynamoDBTableName = "MediaStates";

        public static StateMap SetDefaultState()
        {
            StateMap map = new StateMap()
            {
                EnqueuedToken = null,
                Index = 0,
                Loop = true,
                OffsetInMS = 0,
                PlaybackFinished = false,
                PlaybackIndexChanged = false,
                playOrder = new List<int>(),
                Shuffle = false,
                State = "MENU_MODE",
                Token = null
            };
            return map;
        }



        public static Dictionary<string, string> States
        {
            get
            {
                var stateReturn = new Dictionary<string, string>();
                stateReturn.Add("StartMode", "START_MODE");
                stateReturn.Add("MenuMode", "MENU_MODE");
                stateReturn.Add("PlayMode", "PLAY_MODE");
                stateReturn.Add("ResumeDecisionMode", "RESUME_DECISION_MODE");
                return stateReturn;
            }
        }
    }
}

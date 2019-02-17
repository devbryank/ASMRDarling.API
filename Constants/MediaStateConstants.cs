using System.Collections.Generic;
using ASMRDarling.API.Models;

namespace ASMRDarling.API.Constants
{
    static class MediaStateConstants
    {
        public static StateMap SetDefaultState()
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
                State = "MENU_MODE",
            };

            return map;
        }


        public static Dictionary<string, string> States
        {
            get
            {
                var states = new Dictionary<string, string>();
                states.Add("MenuMode", UserStateConstants.Menu);
                states.Add("MediaMode", UserStateConstants.Media);

#warning do i need this state?
                //statesAdd("ResumeDecisionMode", "RESUME_DECISION_MODE");

                return states;
            }
        }
    }
}

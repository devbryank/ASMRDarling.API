namespace ASMRDarling.API.Templates
{
    /// <summary>
    /// This class contains a collection of Alexa constants
    /// </summary>
    static class AlexaConstants
    {

#warning change name to alexa request constants
        //// request names
        //const string AudioPlayerRequestName = "AudioPlayer";
        public const string Alexa = "Alexa";
        //const string ExceptionRequestName = "System";
        //const string SessionEndedRequestName = "SessionEndedRequest";

        // Invocation constant
        public const string Invocation = "Darling's Gift";


        // Intent slot constant
        public const string MediaItemSlot = "MediaItemName";


        // Request constants
        public const string LaunchRequest = "LaunchRequest";
        public const string IntentRequest = "IntentRequest";


        // Intent constants
        public const string BuiltIn = "AMAZON";
        public const string PlayASMR = "PlayASMR";


        // Sub built in intent constants
        public const string BuiltInHelp = "HelpIntent";
        public const string BuiltInNext = "NextIntent";
        public const string BuiltInPrevious = "PreviousIntent";
        public const string BuiltInResume = "ResumeIntent";
        public const string BuiltInPause = "PauseIntent";
        public const string BuiltInStop = "StopIntent";

        // Sub alexa intent constants
        public const string UserEvent = "UserEvent";
    }
}

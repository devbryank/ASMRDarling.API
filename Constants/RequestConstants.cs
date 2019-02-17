namespace ASMRDarling.API.Constants
{
    class RequestConstants
    {
        // invocation
        public const string Invocation = "Darling's Gift";


        // request types
        public const string Alexa = "Alexa";
        public const string System = "System";
        public const string AudioPlayer = "AudioPlayer";
        public const string LaunchRequest = "LaunchRequest";
        public const string IntentRequest = "IntentRequest";
        public const string SessionEndedRequest = "SessionEndedRequest";


        // intent types
        public const string BuiltIn = "AMAZON";
        public const string PlayAsmr = "PlayASMR";


        // sub built-in intent types
        public const string BuiltInHelp = "HelpIntent";
        public const string BuiltInNext = "NextIntent";
        public const string BuiltInPrevious = "PreviousIntent";
        public const string BuiltInResume = "ResumeIntent";
        public const string BuiltInPause = "PauseIntent";
        public const string BuiltInStop = "StopIntent";


        // sub alexa intent types
        public const string UserEvent = "UserEvent";


        // slot types
        public const string MediaItemSlot = "MediaItemName";
    }
}

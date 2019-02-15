namespace ASMRDarling.API.Templates
{
    /// <summary>
    /// collection of alexa request & intent constants
    /// </summary>
    static class AlexaRequestConstants
    {
        // invocation constant
        public const string Invocation = "Darling's Gift";


        // request constants
        public const string Alexa = "Alexa";
        public const string System = "System";
        public const string AudioPlayer = "AudioPlayer";
        public const string LaunchRequest = "LaunchRequest";
        public const string IntentRequest = "IntentRequest";
        public const string SessionEndedRequest = "SessionEndedRequest";


        // intent constants
        public const string BuiltIn = "AMAZON";
        public const string PlayAsmr = "PlayASMR";


        // slot constants
        public const string MediaItemSlot = "MediaItemName";


        // sub built-in intent constants
        public const string BuiltInHelp = "HelpIntent";
        public const string BuiltInNext = "NextIntent";
        public const string BuiltInPrevious = "PreviousIntent";
        public const string BuiltInResume = "ResumeIntent";
        public const string BuiltInPause = "PauseIntent";
        public const string BuiltInStop = "StopIntent";


        // sub alexa intent constants
        public const string UserEvent = "UserEvent";
    }
}

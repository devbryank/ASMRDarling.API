namespace ASMRDarling.API.Data
{
    /// <summary>
    /// This class contains a collection of Alexa constants
    /// </summary>
    static class AlexaConstants
    {
        // Invocation constant
        public const string Invocation = "Darling's Gift";

        // Request constants
        public const string LaunchRequest = "LaunchRequest";
        public const string IntentRequest = "IntentRequest";

        // Intent constants
        public const string BuiltIn = "Amazon";
        public const string PlayASMR = "PlayASMR";

        // Sub built in intent constants
        public const string BuiltInHelp = "HelpIntent";
        public const string BuiltInNext = "NextIntent";
        public const string BuiltInPrevious = "PreviousIntent";
        public const string BuiltInResume = "ResumeIntent";
        public const string BuiltInPause = "PauseIntent";
        public const string BuiltInStop = "StopIntent";
    }
}

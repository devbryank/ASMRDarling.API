namespace Sonnar
{
    class SkillSetting
    {
        public const string SkillName = "ASMR Darling";

        public const string HashKey = "UserId";
        public const string TableName = "MediaStates";
    }


    class Stage
    {
        public const string Menu = "MENU_MODE";
        public const string Audio = "AUDIO_MODE";
        public const string Video = "VIDEO_MODE";
        public const string Pause = "PAUSE_MODE";
    }


    static class AlexaRequestType
    {
        public const string CustomIntent = "PlayASMR";
        public const string CustomSlot = "MediaItemName";

        public const string AlexaRequest = "Alexa";
        public const string SystemRequest = "System";

        public const string LaunchRequest = "LaunchRequest";
        public const string IntentRequest = "IntentRequest";

        public const string UserEventRequest = "UserEvent";
        public const string AudioPlayerRequest = "AudioPlayer";
        public const string SessionEndedRequest = "SessionEndedRequest";

        public const string BuiltInIntent = "AMAZON";
        public const string BuiltInHelp = "HelpIntent";
        public const string BuiltInNext = "NextIntent";
        public const string BuiltInPrevious = "PreviousIntent";
        public const string BuiltInResume = "ResumeIntent";
        public const string BuiltInCancel = "CancelIntent";
        public const string BuiltInPause = "PauseIntent";
        public const string BuiltInStop = "StopIntent";
        public const string BuiltInRepeat = "RepeatIntent";
        public const string BuiltInLoopOn = "LoopOnIntent";
        public const string BuiltInLoopOff = "LoopOffIntent";
        public const string BuiltInShuffleOn = "ShuffleOnIntent";
        public const string BuiltInShuffleOff = "ShuffleOffIntent";
        public const string BuiltInStartOver = "StartOverIntent";
        public const string BuiltInFallback = "FallbackIntent";

        public const string PlaybackStarted = "PlaybackStarted";
        public const string PlaybackStopped = "PlaybackStopped";
        public const string PlaybackNearlyFinished = "PlaybackNearlyFinished";
        public const string PlaybackFinished = "PlaybackFinished";
        public const string PlaybackFailed = "PlaybackFailed";
    }
}

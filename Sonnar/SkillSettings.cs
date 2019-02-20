namespace Sonnar
{
    class SkillSettings
    {
        public const string SkillName = "Darling's gift";   // custom invocation & skill name

        public const string HashKey = "UserId";             // custom hash key for dynamodb
        public const string TableName = "MediaStates";      // custom dynamodb table name

        public const string DbAccessKey = "AKIAJOMVNVDUCK3GWQPQ";                       // custom dynamodb access key
        public const string DbSecretKey = "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";   // custom dynamodb secret key
    }


    static class AlexaRequests
    {
        public const string CustomIntent = "PlayASMR";      // custom intent name
        public const string CustomSlot = "MediaItemName";   // custom slot name

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

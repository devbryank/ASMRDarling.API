namespace Sonnar.Templates
{
    class SpeechTemplate
    {
        public const string WelcomeBack = "Welcome back. ";
        public static string WelcomeNew = $"Welcome to {SkillSettings.SkillName}. ";
        public const string Intro = "Say the name of recording, or list to hear about available recordings, or help to find out other options. ";

        public const string MoreOptions = "You can either say play followed by a recording name, or say list, help, and shuffle on and off. ";
        public const string NoNext = "Next media item is not available. You can say resume to keep play, or say list to hear more options ";
        public const string NoPrevious = "Previous media item is not available. You can say resume to keep play, or say list to hear more options ";

        public const string SeeYouSoon = "See you soon. ";

        public const string NoCommand = "The command you have given is not supported. Please say list, or play followed by name of the media. ";
        public const string NotUnderstand = "I did not understand. ";
        public const string IntentUnknown = "I didn't understand your intent. Please try again. ";
        public const string RequestUnknown = "I didn't understand your request. Please try again. ";
        public const string SystemException = "Unable to process your request, due to system failures. Please try again. ";


#warning add foreach loop to concat
        public static string ListItems = "Here is the list of ASMR items. What is ASMR. " +
                                         "10 triggers to help you sleep. " +
                                         "20 triggers to help you sleep. " +
                                         "100 triggers to help you sleep. " +
                                         "A to z triggers to help you sleep. " +
                                         "Brushing the microphone. " +
                                         "Relaxing head massage. " +
                                         "Relaxing scalp massage. " +
                                         "Whispered tapping and scratching. " +
                                         "Close up personal attention for you to sleep. " +
                                         "Which item do you want me to play? ";
    }
}

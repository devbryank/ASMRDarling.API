using Sonnar.Components;

namespace Sonnar.Templates
{
    class SpeechTemplate
    {
        public const string WelcomeBack = "Welcome back. ";
        public static string WelcomeNew = $"Welcome to {SkillSetting.SkillName}. ";

        public const string Help = "You can either say a recording name, list, or shuffle. ";
        public const string Intro = "Say the name of recording, or list to hear about available recordings, or help to find out other options. ";
        public const string MoreOptions = "You can either say a recording name, list, shuffle, or help. ";

        public static string NoUnderstand
        {
            get
            {
                return GetPropmpt();
            }
            set
            {
            }
        }
  
        public const string NoNext = "Next recording is not available. You can say resume to keep play, or say list to hear more options ";
        public const string NoPrevious = "Previous recording is not available. You can say resume to keep play, or say list to hear more options ";

        public const string GoodBye = "Good Bye. ";
        public const string SeeYouSoon = "See you soon. ";
        public const string SeeYouNextTime = "See you next time. ";

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

        public const string SystemException = "The skill is not working at the moment. Please try again later. ";


        public static string GetPropmpt()
        {
            string reprompt = string.Empty;

            switch (Core.State.UserState.NumReprompt)
            {
                case 0:
                    reprompt = "I didn't understand. Please try again. ";
                    break;
                case 1:
                    reprompt = "You can say a name of recording, list, or help. ";
                    break;
                case 2:
                    reprompt = "If you like to play any recording, just say shuffle. ";
                    break;
                case 3:
                    reprompt = "I suggest you to say help. ";
                    break;
                case 5:
                    reprompt = "Sorry. Something went wrong, please try again. ";
                    break;
            }

            Core.State.UserState.NumReprompt++;

            return reprompt;
        }
    }
}

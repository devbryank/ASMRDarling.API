using Alexa.NET.Response;

namespace ASMRDarling.API.Data
{
    /// <summary>
    /// collection of preset ssml speeches
    /// </summary>
    static class SsmlTemplate
    {
        // when requested device does not have a display interface
        const string NoDisplayLaunchSpeech = "<speak>" +
                                                  "<p>Welcome to Darling's gift.</p>" +
                                                  "<p>To begin,</p>" +
                                                  "<p>you can say things like, play what is ASMR.</p>" +
                                                  "<p>Otherwise, say help to hear about more options.</p>" +
                                             "</speak>";


        // when requested device has a display interface
        const string DisplayLaunchSpeech = "<speak>" +
                                                "<p>Welcome to Darling's gift.</p>" +
                                                "<p>To play a video,</p>" +
                                                "<p>you can tap on any of the thumbnails.</p>" +
                                                "<p>Otherwise, you can simply say the name of the video you want to play.</p>" +
                                           "</speak>";


        // when user says help (giving out more option to choose)
        const string MediaItemListSpeech = "<speak>" +
                                                "<p>Here is the list of ASMR items.</p>" +
                                                "<p>What is ASMR.</p>" +
                                                "<p>10 triggers to help you sleep.</p>" +
                                                "<p>20 triggers to help you sleep.</p>" +
                                                "<p>100 triggers to help you sleep.</p>" +
                                                "<p>A to z triggers to help you sleep.</p>" +
                                                "<p>Brushing the microphone.</p>" +
                                                "<p>Relaxing head massage.</p>" +
                                                "<p>Relaxing scalp massage.</p>" +
                                                "<p>Whispered tapping and scratching.</p>" +
                                                "<p>Close up personal attention for you to sleep.</p>" +
                                                "<p>Which item do you want me to play?</p>" +
                                           "</speak>";


        // when there is no next item (end of the queue)
        const string MediaPlayerNoNextSpeech = "<speak>" +
                                                    "<p>Next media item is not available.</p>" +
                                                    "<p>You can say resume to keep listen to the current media.</p>" +
                                                    "<p>Otherwise, please choose another option by saying help,</p>" +
                                                    "<p>or play followed by name of the media.</p>" +
                                               "</speak>";


        // when there is no previous item (beginning of the queue)
        const string MediaPlayerNoPreviousSpeech = "<speak>" +
                                                        "<p>Previous media item is not available.</p>" +
                                                        "<p>You can say resume to keep listen to the current media.</p>" +
                                                        "<p>Otherwise, please choose another option by saying help,</p>" +
                                                        "<p>or play followed by name of the media.</p>" +
                                                   "</speak>";


        // when user interrupts the stream but was not recognized
        const string MediaPlayerControlSpeech = "<speak>" +
                                                     "<p>While the media is in play,</p>" +
                                                     "<p>you can say help, next, previous, resume, pause, stop to control.</p>" +
                                                "</speak>";


        // when requested command is not supported (saying next while in menu selection state)
        const string NoCommandSpeech = "<speak>" +
                                            "<p>The command you have given is not supported.</p>" +
                                            "<p>Please say help, or play followed by name of the media.</p>" +
                                       "</speak>";


        // when request is not recognized, asking for other options
        const string NotSupportedSpeech = "<speak>" +
                                               "<p>Sorry, I didn't get your request.</p>" +
                                               "<p>Please say help, or play followed by name of the media.</p>" +
                                          "</speak>";


        // when exception is caught in the middle of the processing request
        const string SystemExceptionSpeech = "<speak>" +
                                                  "<p>Unable to process your request.</p>" +
                                                  "<p>Please contact the developer, if the problem persists.</p>" +
                                             "</speak>";


        // return launch request speech
        public static SsmlOutputSpeech LaunchSpeech(bool? hasDisplay)
        {
            if (hasDisplay == true)
                return new SsmlOutputSpeech() { Ssml = DisplayLaunchSpeech };
            else
                return new SsmlOutputSpeech() { Ssml = NoDisplayLaunchSpeech };
        }


        // return help request speech
        public static SsmlOutputSpeech HelpSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaItemListSpeech };
        }


        // return default media (control) speech
        public static SsmlOutputSpeech ControlMediaSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaPlayerControlSpeech };
        }


        // return no next media speech
        public static SsmlOutputSpeech NoNextMediaSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaPlayerNoNextSpeech };
        }


        // return no previous media speech
        public static SsmlOutputSpeech NoPreviousMediaSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaPlayerNoPreviousSpeech };
        }


        // return default command fallback speech
        public static SsmlOutputSpeech CommandFallbackSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = NoCommandSpeech };
        }


        // return default request fallback speech
        public static SsmlOutputSpeech RequestFallbackSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = NotSupportedSpeech };
        }


        // return system exception speech
        public static SsmlOutputSpeech FatalExceptionSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = SystemExceptionSpeech };
        }
    }
}

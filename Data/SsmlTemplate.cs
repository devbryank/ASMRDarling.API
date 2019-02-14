using Alexa.NET.Response;

namespace ASMRDarling.API.Data
{
    /// <summary>
    /// This class contains a collection of SSML preset speeches
    /// </summary>
    static class SsmlTemplate
    {
        const string LaunchAudioSpeech = "<speak>" +
                                              "<p>Welcome to Darling's gift.</p>" +
                                              "<p>To begin,</p>" +
                                              "<p>you can say things like, play 10 triggers to help you sleep.</p>" +
                                              "<p>Otherwise, say help to hear about more options.</p>" +
                                         "</speak>";

        const string LaunchVideoSpeech = "<speak>" +
                                              "<p>Welcome to Darling's gift.</p>" +
                                              "<p>To play a video,</p>" +
                                              "<p>you can tap on any of the thumbnails.</p>" +
                                              "<p>Otherwise, you can simply say the name of the video you want to play.</p>" +
                                         "</speak>";

        const string MenuHelpSpeech = "<speak>" +
                                           "<p>Here is the list of ASMR clips.</p>" +
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

        const string MediaPlayerNoNextSpeech = "<speak>" +
                                                    "<p>Next media item is not available.</p>" +
                                                    "<p>You can say resume to play the media.</p>" +
                                                    "<p>Otherwise, please choose another option by saying help,</p>" +
                                                    "<p>or play followed by name of the media.</p>" +
                                               "</speak>";

        const string MediaPlayerNoPreviousSpeech = "<speak>" +
                                                        "<p>Previous media item is not available.</p>" +
                                                        "<p>You can say resume to play the media.</p>" +
                                                        "<p>Otherwise, please choose another option by saying help,</p>" +
                                                        "<p>or play followed by name of the media.</p>" +
                                                   "</speak>";

        const string MediaPlayerDefaultSpeech = "<speak>" +
                                                     "<p>While a media is in play,</p>" +
                                                     "<p>you can say help, next, previous, resume, pause, stop to control.</p>" +
                                                "</speak>";

        const string NoCommandSpeech = "<speak>" +
                                            "<p>The command you have given is not supported.</p>" +
                                            "<p>Please say help, or play followed by name of the media.</p>" +
                                       "</speak>";

        const string NotSupportedSpeech = "<speak>" +
                                               "<p>Sorry, I didn't get your request.</p>" +
                                               "<p>Please try again later.</p>" +
                                          "</speak>";

        const string SystemExceptionSpeech = "<speak>" +
                                                  "<p>Unable to process request.</p>" +
                                                  "<p>Please contact the developer, if the problem persists.</p>" +
                                             "</speak>";


        // Return launch request speech
        public static SsmlOutputSpeech LaunchSpeech(bool? hasDisplay)
        {
            if (hasDisplay == true)
                return new SsmlOutputSpeech() { Ssml = LaunchVideoSpeech };
            else
                return new SsmlOutputSpeech() { Ssml = LaunchAudioSpeech };
        }


        // Return help request speech
        public static SsmlOutputSpeech HelpSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MenuHelpSpeech };
        }


        // Return default media player speech
        public static SsmlOutputSpeech ControlMediaSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaPlayerDefaultSpeech };
        }


        // Return no next media player speech
        public static SsmlOutputSpeech NoNextMediaSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaPlayerNoNextSpeech };
        }


        // Return no previous media player speech
        public static SsmlOutputSpeech NoPreviousMediaSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaPlayerNoPreviousSpeech };
        }


        // Return default fallback speech
        public static SsmlOutputSpeech FallbackSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = NotSupportedSpeech };
        }


        // Return command fallback speech
        public static SsmlOutputSpeech CommandFallbackSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = NoCommandSpeech };
        }


        // Return when system exception speech
        public static SsmlOutputSpeech ExceptionSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = SystemExceptionSpeech };
        }
    }
}

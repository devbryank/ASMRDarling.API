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

        const string SystemExceptionSpeech = "<speak>" +
                                                    "<p>Unable to get a response from the server.</p>" +
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


        // Return when system exception speech
        public static SsmlOutputSpeech ExceptionSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = SystemExceptionSpeech };
        }
    }
}

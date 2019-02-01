using Alexa.NET.Response;

namespace ASMRDarling.API.Templates
{
    public class SsmlTemplate
    {
        const string LaunchAudioSpeech = "<speak>" +
                                             "<p>Welcome to Darling's gift.</p>" +
                                             "<p>To begin,</p>" +
                                             "<p>you can say things like, play 10 triggers to help you sleep.</p>" +
                                             "<p>Otherwise, say help to hear about more options.</p>" +
                                         "</speak>";

        const string LaunchVideoSpeech = "<speak>" +
                                             "<p>Welcome to Darling's gift.</p>" +
                                             "<p>To play a clip,</p>" +
                                             "<p>you can tap on any of the thumbnails.</p>" +
                                             "<p>Otherwise, you can simply say the name of the clip you want to play.</p>" +
                                         "</speak>";

        const string ExceptionAudioSpeech = "<speak>" +
                                                 "<p>Sorry, I didn't get your intention.</p>" +
                                                 "<p>Please try again, or say help to hear about other options.</p>" +
                                            "</speak>";


        // Return launch request speech
        public static SsmlOutputSpeech LaunchSpeech(bool? hasDisplay)
        {
            if (hasDisplay == true)
                return new SsmlOutputSpeech() { Ssml = LaunchVideoSpeech };
            else
                return new SsmlOutputSpeech() { Ssml = LaunchAudioSpeech };
        }


        // Return when exceptions caught
        public static SsmlOutputSpeech ExceptionSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = ExceptionAudioSpeech };
        }
    }
}

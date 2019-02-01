using Alexa.NET.Response;

namespace ASMRDarling.API.Builders
{
    public class SsmlBuilder
    {

        const string HelpAudioSpeech = "<speak><amazon:effect name='whispered'><prosody rate='slow'>You can say list, or options to hear about ASMR recordings that I can offer.</prosody></amazon:effect></speak>";


        public static SsmlOutputSpeech BuildSpeech(string speech)
        {
            return new SsmlOutputSpeech() { Ssml = speech };
        }
















     

        public static SsmlOutputSpeech HelpSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = HelpAudioSpeech };
        }
    }
}

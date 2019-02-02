﻿using Alexa.NET.Response;

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

        const string HelpAudioSpeech = "<speak>" +
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
                                       "</speak>";

        const string MediaPlayerDefaultSpeech = "<speak>" +
                                                     "<p>While a media is in play,</p>" +
                                                     "<p>you can say help, next, previous, resume, pause, stop to control.</p>" +
                                                "</speak>";

        const string MediaPlayerNoNextAudioSpeech = "<speak>" +
                                                         "<p>Next clip is not available.</p>" +
                                                         "<p>Please choose other options.</p>" +
                                                    "</speak>";

        const string MediaPlayerNoPreviousAudioSpeech = "<speak>" +
                                                             "<p>Previous clip is not available.</p>" +
                                                             "<p>Please choose other options.</p>" +
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


        // Return help request speech
        public static SsmlOutputSpeech HelpSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = HelpAudioSpeech };
        }


        // Return media player default request speech
        public static SsmlOutputSpeech MediaPlayerControlSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaPlayerDefaultSpeech };
        }


        // Return no next media item intent speech
        public static SsmlOutputSpeech MediaPlayerNoPreviousSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaPlayerNoPreviousAudioSpeech };
        }


        // Return no next media item intent speech
        public static SsmlOutputSpeech MediaPlayerNoNextSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = MediaPlayerNoNextAudioSpeech };
        }


        // Return when exceptions caught
        public static SsmlOutputSpeech ExceptionSpeech()
        {
            return new SsmlOutputSpeech() { Ssml = ExceptionAudioSpeech };
        }
    }
}

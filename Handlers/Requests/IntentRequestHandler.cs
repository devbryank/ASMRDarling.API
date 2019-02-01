﻿using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class IntentRequestHandler : IIntentRequestHandler
    {
        // Intent names
        const string PlayIntentName = "PlayMedia";
        const string BuiltInIntentName = "AMAZON";
        const string AudioPlayerIntentName = "AudioPlayer";

        // Intent handlers
        readonly IPlayMediaIntentHandler playMediaIntentHandler;
        readonly IBuiltInIntentHandler builtInIntentHandler;
        readonly IAudioPlayerIntentHandler audioPlayerIntentHandler;


        // Constructor
        public IntentRequestHandler()
        {
            playMediaIntentHandler = new PlayMediaIntentHandler();
            builtInIntentHandler = new BuiltInIntentHandler();
            audioPlayerIntentHandler = new AudioPlayerIntentHandler();
        }


        // Request handler
        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent request handling started");

            // Get intent request
            var request = input.Request as IntentRequest;
            logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent requested: {request.Intent.Name}");

            // Declare response to return
            Intent intent = request.Intent;
            SkillResponse response = new SkillResponse();
            SsmlOutputSpeech output = new SsmlOutputSpeech();

            // Direct intent into the matching handler
            switch (intent.Name)
            {
                // Handle play media intent
                case PlayIntentName:
                    logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {PlayIntentName} handler");
                    response = await playMediaIntentHandler.HandleIntent(intent, session, logger);
                    break;

                // Handle other intents like built in & audio player intents
                default:
                    var intentNamePartials = intent.Name.Split('.');

                    if (intentNamePartials[0].Equals(BuiltInIntentName))
                    {
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {BuiltInIntentName} handler");
                        response = await builtInIntentHandler.HandleIntent(intent, session, logger);
                    }
                    else if (intentNamePartials[0].Equals(AudioPlayerIntentName))
                    {
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {AudioPlayerIntentName} handler");
                        response = await audioPlayerIntentHandler.HandleIntent(intent, session, logger);
                    }
                    else
                    {
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent was not recognized, directing intent into the default case");
                        output = SsmlTemplate.ExceptionSpeech();
                        response = ResponseBuilder.Ask(output, null);
                    }
                    break;
            }

            return response;
        }


        //AudioPlayer.ClearQueue
        //AudioPlayer.PlaybackStarted   say playing filename
        //AudioPlayer.PlaybackStopped  say play paused
        //AudioPlayer.PlaybackFinished
        //AudioPlayer.PlaybackFailed   should add a fallback?
    }
}

using Amazon.Lambda.Core;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Sonnar.Core;
using Sonnar.Helpers;
using ASMRDarling.API.Models;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Constants;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ASMRDarling.API
{
    public class Function : Core
    {
        public async Task<SkillResponse> FunctionHandler(APLSkillRequest input, ILambdaContext context)
        {
            // initialize
            Init(input, context);
            AddRequestConverters();
            SetDisplayHeight();


            // start logging
            Logger.Write($"[Function.FunctionHandler()] Alexa skill {RequestConstants.Invocation} launched");
            Logger.Write($"[Function.FunctionHandler()] Input details: {JsonConvert.SerializeObject(Input)}");
            Logger.Write($"[Function.FunctionHandler()] Context details: {JsonConvert.SerializeObject(Context)}");
            Logger.Write($"[Function.FunctionHandler()] Diplay availability: {Device.HasScreen}");

            if (Device.HasScreen)
                Logger.Write($"[Function.FunctionHandler()] Diplay is round: {Device.IsRound}");


            // get main & sub request types
            string requestType = Input.Request.Type;
            string[] requestTypes = requestType.Split('.');
            string mainRequestType = requestTypes[0];
            string subRequestType = requestTypes.Length > 1 ? requestTypes[requestTypes.Length - 1] : null;

            Logger.Write($"[Function.FunctionHandler()] Main request type: {mainRequestType}");
            if (subRequestType != null)
                Logger.Write($"[Function.FunctionHandler()] Sub request type: {subRequestType} derived from {requestType}");


            // initialize & load database for managing user states
            Logger.Write($"[Function.FunctionHandler()] Acquiring user's media state from database");

            MediaStateHelper mediaStateHelper = new MediaStateHelper();
            await mediaStateHelper.VerifyTable();

            string userId = input.Session != null ? input.Session.User.UserId : input.Context.System.User.UserId;
            var lastState = await mediaStateHelper.GetMediaState(userId);
            var currentState = new MediaState() { UserId = userId };
            currentState.State = lastState.State;

            Logger.Write($"[Function.FunctionHandler()] Current user state: {JsonConvert.SerializeObject(currentState)}");










            return Response.Build();
        }


        void AddRequestConverters()
        {
            new UserEventRequestHandler().AddToRequestConverter();
            //new SystemExceptionEncounteredRequestTypeConverter().AddToRequestConverter();
        }


        void SetDisplayHeight()
        {
            try
            {
                DisplayHelper.BaseDeviceHeight = Device.IsRound ? 480f : 1080f;
            }
            catch (Exception ex)
            {
                Logger.Write($"[Function.SetDisplayHeight()] Exception caught, unable to set the device height");
                Logger.Write($"[Function.SetDisplayHeight()] Exception details: {JsonConvert.SerializeObject(ex)}");
            }
        }
    }
}

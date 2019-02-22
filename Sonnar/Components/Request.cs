using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;

namespace Sonnar.Components
{
    class Request
    {
        protected APLSkillRequest Skill { get; private set; }

        protected string MainRequestType { get; private set; }
        protected string SubRequestType { get; private set; }

        protected string MainIntentType { get; private set; }
        protected string SubIntentType { get; private set; }


        public Request(APLSkillRequest request)
        {
            Skill = new APLSkillRequest();
            Skill = request;

            SetRequestTypes();
            if (request.Request.GetType() == typeof(IntentRequest))
                SetIntentTypes();

            Core.Logger.Write("Request.Request()", $"Request details: {JsonConvert.SerializeObject(request)}");
        }


        public Session GetSession() { return Skill.Session; }
        public APLSkillRequest GetRequest() { return Skill; }

        public string GetMainRequestType() { return MainRequestType; }
        public string GetSubRequestType() { return SubRequestType; }
        public string GetMainIntentType() { return MainIntentType; }
        public string GetSubIntentType() { return SubIntentType; }


        private void SetRequestTypes()
        {
            string[] requestTypes = Skill.Request.Type.Split(".");

            MainRequestType = requestTypes[0];
            SubRequestType = requestTypes.Length > 1 ? requestTypes[requestTypes.Length - 1] : null;

            Core.Logger.Write("Request.SetRequestTypes()", $"Main request type: {MainRequestType}");
            if (SubRequestType != null)
                Core.Logger.Write("Request.SetRequestTypes()", $"Sub request type: {SubRequestType}");
        }


        private void SetIntentTypes()
        {
            IntentRequest request = Skill.Request as IntentRequest;
            string[] intentTypes = request.Intent.Name.Split(".");

            MainIntentType = intentTypes[0];
            SubIntentType = intentTypes.Length > 1 ? intentTypes[intentTypes.Length - 1] : null;

            Core.Logger.Write("Request.SetIntentTypes()", $"Main intent type: {MainIntentType}");
            if (SubIntentType != null)
                Core.Logger.Write("Request.SetIntentTypes()", $"Sub intent type: {SubIntentType}");
        }
    }
}

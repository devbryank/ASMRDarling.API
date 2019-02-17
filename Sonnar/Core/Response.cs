using Alexa.NET;
using Alexa.NET.Response;

namespace Sonnar.Core
{
    public class Response
    {
        SkillResponse response;


        public Response()
        {
            Init();
        }


        void Init()
        {
            response = new SkillResponse();
            response = ResponseBuilder.Empty();
        }


        public SkillResponse Build()
        {
            return response;
        }
    }
}

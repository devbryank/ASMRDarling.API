using Amazon.Lambda.Core;
using Alexa.NET.Request;

namespace Sonnar.Core
{
    public class Core
    {
        public static APLSkillRequest Input;
        public static ILambdaContext Context;

        public static Logger Logger;
        public static Device Device;
        public static Response Response;


        public static void Init(APLSkillRequest input, ILambdaContext context)
        {
            Input = input;
            Context = context;

            Logger = new Logger();
            Device = new Device();
            Response = new Response();
        }
    }
}

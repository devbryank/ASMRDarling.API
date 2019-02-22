using Alexa.NET.Request;
using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace Sonnar.Components
{
    class Core
    {
        public static Logger Logger;
        public static Request Request;
        public static Response Response;

        public static State State;
        public static Device Device;
        public static Context Context;
        public static Database Database;


        public static async Task Init(APLSkillRequest input, ILambdaContext context)
        {
            new UserEventRequestHandler().AddToRequestConverter();
            //new SystemExceptionEncounteredRequestTypeConverter().AddToRequestConverter();

            // initialize logger
            Logger = new Logger(context.Logger);
            Logger.Write("Core.Init()", $"{SkillSetting.SkillName} started");

            // get user id
            string userId = input.Session != null ? input.Session.User.UserId : input.Context.System.User.UserId;

            // initialize components
            Request = new Request(input);
            Response = new Response();
            Device = new Device(input);
            Context = new Context(context);

            // initialize database context
            Database = new Database(userId);
            await Database.VerifyTable();
            State = await Database.GetState();

            Logger.Write("Core.Init()", "Sonnar Core library initialization completed");
        }
    }
}

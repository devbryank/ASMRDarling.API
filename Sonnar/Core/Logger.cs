using Amazon.Lambda.Core;

namespace Sonnar.Core
{
    public class Logger
    {
        ILambdaLogger logger;


        public Logger()
        {
            Init();
        }


        void Init()
        {
            logger = Core.Context.Logger;
        }


        public void Write(string log)
        {
            logger.LogLine(log);
        }
    }
}

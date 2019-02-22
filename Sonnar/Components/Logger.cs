using Amazon.Lambda.Core;

namespace Sonnar.Components
{
    class Logger
    {
        protected ILambdaLogger SkillLogger { get; private set; }
        public Logger(ILambdaLogger logger) { SkillLogger = logger; }

        public void Write(string taskName, string log)
        {
            SkillLogger.LogLine($"[{taskName}] {log}");
        }
    }
}

using Amazon.Lambda.Core;

namespace Sonnar.Components
{
    class Context
    {
        protected ILambdaContext SkillContext { get; private set; }


        public Context(ILambdaContext context)
        {
            SkillContext = context;
        }
    }
}

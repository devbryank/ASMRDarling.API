using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ASMRDarling.API
{
    /// <summary>
    /// entry point of the lambda function
    /// </summary>
    public class Function
    {
        // handle request from alexa
        public async Task<SkillResponse> FunctionHandler(APLSkillRequest input, ILambdaContext context)
        {
           

            return response;
        }
    }
}

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace cloudpart2
{
    public class TestHttpFunction
    {
        private readonly ILogger<TestHttpFunction> _logger;

        public TestHttpFunction(ILogger<TestHttpFunction> logger)
        {
            _logger = logger;
        }

        [Function("TestHttpFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("HTTP trigger function executed.");
            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            response.WriteString("Function executed successfully!");
            return response;
        }
    }
}
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Files.Shares;

namespace cloudpart2
{
    public class WriteToAzureFiles
    {
        private readonly ShareClient _shareClient;
        private readonly ILogger _logger;

        public WriteToAzureFiles(ShareClient shareClient, ILoggerFactory loggerFactory)
        {
            _shareClient = shareClient;
            _logger = loggerFactory.CreateLogger<WriteToAzureFiles>();
        }

        [Function("WriteToAzureFiles")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Writing data to Azure Files.");
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("File data"));
            var directoryClient = _shareClient.GetDirectoryClient("");
            var fileClient = directoryClient.GetFileClient("samplefile.txt");
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadRangeAsync(new Azure.HttpRange(0, stream.Length), stream);
            _logger.LogInformation("File written successfully.");
        }
    }
}
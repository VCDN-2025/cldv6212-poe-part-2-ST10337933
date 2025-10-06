using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;

namespace cloudpart2
{
    public class WriteToBlobStorage
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger<WriteToBlobStorage> _logger;

        public WriteToBlobStorage(BlobContainerClient blobContainerClient, ILogger<WriteToBlobStorage> logger)
        {
            _blobContainerClient = blobContainerClient;
            _logger = logger;
        }

        [Function("WriteToBlobStorage")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Writing data to Blob Storage.");
            string blobName = "sampleblob.txt";
            var blobClient = _blobContainerClient.GetBlobClient(blobName);

            // Check if blob exists and overwrite if desired
            if (await blobClient.ExistsAsync())
            {
                _logger.LogInformation("Blob {BlobName} already exists, overwriting.", blobName);
                await blobClient.DeleteIfExistsAsync(); // Optional: Delete first to overwrite
            }

            using (var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Blob data")))
            {
                await blobClient.UploadAsync(ms, overwrite: true); // Overwrite if exists
            }

            _logger.LogInformation("Blob uploaded successfully.");
        }
    }
}
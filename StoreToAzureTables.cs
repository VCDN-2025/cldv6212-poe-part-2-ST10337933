using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;

namespace cloudpart2
{
    public class StoreToAzureTables
    {
        private readonly TableClient _tableClient;
        private readonly ILogger _logger;

        public StoreToAzureTables(TableClient tableClient, ILoggerFactory loggerFactory)
        {
            _tableClient = tableClient;
            _logger = loggerFactory.CreateLogger<StoreToAzureTables>();
        }

        [Function("StoreToAzureTables")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Storing data to Azure Tables.");
            string rowKey = $"Row1_{DateTimeOffset.UtcNow.Ticks}";
            var entity = new TableEntity("Partition1", rowKey)
            {
                { "Data", "Sample transaction data" }
            };
            await _tableClient.AddEntityAsync(entity);
            _logger.LogInformation("Data stored successfully with RowKey: {RowKey}", rowKey);
        }
    }
}
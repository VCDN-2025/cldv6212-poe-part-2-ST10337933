using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues;

namespace cloudpart2
{
    public class QueueTransaction
    {
        private readonly QueueClient _queueClient;
        private readonly ILogger _logger;

        public QueueTransaction(QueueClient queueClient, ILoggerFactory loggerFactory)
        {
            _queueClient = queueClient;
            _logger = loggerFactory.CreateLogger<QueueTransaction>();
        }

        [Function("QueueTransaction")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Sending message to Queue.");
            await _queueClient.SendMessageAsync("New transaction processed");
            var response = await _queueClient.ReceiveMessageAsync();
            if (response.Value != null)
            {
                await _queueClient.DeleteMessageAsync(response.Value.MessageId, response.Value.PopReceipt);
                _logger.LogInformation("Message processed: {Message}", response.Value.MessageText);
            }
        }
    }
}
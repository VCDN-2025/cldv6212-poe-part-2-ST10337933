using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;

public class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureFunctionsWebApplication(webApplication =>
            {
                // Empty for now since using TimerTrigger
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<TableClient>(s =>
                {
                    string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                    return new TableClient(connectionString, "TransactionTable");
                });
                services.AddSingleton<BlobContainerClient>(s =>
                {
                    string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                    return new BlobContainerClient(connectionString, "transactionblobs");
                });
                services.AddSingleton<QueueClient>(s =>
                {
                    string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                    return new QueueClient(connectionString, "transactionqueue");
                });
                services.AddSingleton<ShareClient>(s =>
                {
                    string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                    return new ShareClient(connectionString, "transactionshare");
                });
            })
            .Build();

        host.Run();
    }
}
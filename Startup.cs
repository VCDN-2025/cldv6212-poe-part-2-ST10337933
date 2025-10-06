using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;

public static class Startup
{
    public static void Configure(IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            string connectionString = context.Configuration["AzureWebJobsStorage"];
            services.AddSingleton<TableClient>(s => new TableClient(connectionString, "TransactionTable"));
            services.AddSingleton<BlobContainerClient>(s => new BlobContainerClient(connectionString, "transactionblobs"));
            services.AddSingleton<QueueClient>(s => new QueueClient(connectionString, "transactionqueue"));
            services.AddSingleton<ShareClient>(s => new ShareClient(connectionString, "transactionshare"));
        });
    }
}
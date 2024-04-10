using etlbackgroundworker.Models.Config;
using etlbackgroundworker.Services;
using etlbackgroundworker.Services.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
.ConfigureServices((hostContext, services) => {
    services.AddSingleton<JobSchedulerService>();
    services.AddSingleton<IJobSchedulerService, JobSchedulerService>(provider => provider.GetRequiredService<JobSchedulerService>());
    services.AddHostedService(provider => provider.GetRequiredService<JobSchedulerService>());
    services.AddHostedService<JobEnqueuerService>();

    // configuration section
    IConfiguration configuration = hostContext.Configuration;
    services.Configure<AzureBlobStorageSettings>(configuration.GetSection("AzureBlobStorage"));
    services.Configure<CosmosDbSettings>(configuration.GetSection("CosmosDb"));

}).Build();

await builder.RunAsync();
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using etlbackgroundworker.Models;
using etlbackgroundworker.Models.Config;
using etlbackgroundworker.Services.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace etlbackgroundworker.Services
{
    public class JobEnqueuerService : IHostedService
    {
        private readonly IJobSchedulerService _jobSchedulerService;
        private readonly IOptions<AzureBlobStorageSettings> _azureBlobStorageSettings;

        public JobEnqueuerService(IJobSchedulerService jobSchedulerService, IOptions<AzureBlobStorageSettings> azureBlobStorageSettings)
        {
            _jobSchedulerService = jobSchedulerService;
            _azureBlobStorageSettings = azureBlobStorageSettings;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var job = new ScheduleJob
            {
                Task = (token) => 
                {
                    // await Task.Delay(2000, cancellationToken);
                    throw new Exception("Somenting failed");
                },
                NextRunTime = DateTime.UtcNow.AddSeconds(40),
                Retries = 2,
                DelayAfterRetray = 20
            };

            _jobSchedulerService.ScheduleJob(job);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using etlbackgroundworker.Models;
using etlbackgroundworker.Services.Core;
using Microsoft.Extensions.Hosting;

namespace etlbackgroundworker.Services
{
    public class NotificationService : BackgroundService
    {
        private readonly IDataProcessor _dataProcessor;
        private readonly JobSchedulerService _jobSchedulerService;

        public NotificationService(IDataProcessor dataProcessor, JobSchedulerService jobSchedulerService)
        {
            _dataProcessor = dataProcessor;
            _jobSchedulerService = jobSchedulerService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _jobSchedulerService.ScheduleJob(new ScheduleJob {
                    NextRunTime = DateTime.UtcNow.AddMinutes(2),
                    Task = async (cancellationToek) => {
                        Console.WriteLine("Scheduled job was executed");
                        await Task.CompletedTask;
                    }
                });
                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }
        }
        
    }
}
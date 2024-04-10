using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using etlbackgroundworker.Models;
using etlbackgroundworker.Services.Core;
using Microsoft.Extensions.Hosting;

namespace etlbackgroundworker.Services
{
    public class JobSchedulerService : BackgroundService, IJobSchedulerService
    {
        private readonly List<ScheduleJob> _jobs = new List<ScheduleJob>();
        private readonly Comparison<ScheduleJob> OrderJobs = (a, b) => a.NextRunTime.CompareTo(b.NextRunTime);

        public void ScheduleJob(ScheduleJob job)
        {
            _jobs.Add(job);
            _jobs.Sort(OrderJobs);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<ScheduleJob> jobsToRemove = new List<ScheduleJob>();
                foreach (var job in _jobs.ToList())
                {
                    var now = DateTime.UtcNow;
                    if (now >= job.NextRunTime)
                    {
                        while (job.Retries > 0)
                        {

                            try
                            {
                                await job.Task(stoppingToken);
                                jobsToRemove.Add(job);
                                break;
                            }
                            catch (System.Exception e)
                            {
                                job.Retries--;
                                if (job.Retries > 0)
                                {
                                    await Task.Delay(TimeSpan.FromSeconds(job.DelayAfterRetray), stoppingToken);
                                }
                            }

                        }
                        if (job.Retries <= 0 && !jobsToRemove.Contains(job))
                        {
                            jobsToRemove.Add(job);
                        }
                    }

                }
                // remove executed jobs
                foreach (var job in jobsToRemove)
                {
                    _jobs.Remove(job);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }


}
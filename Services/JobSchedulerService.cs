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
            Console.WriteLine("Job added");
            _jobs.Add(job);
            _jobs.Sort(OrderJobs);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<ScheduleJob> jobsToRemove = new List<ScheduleJob>();
                Console.WriteLine($"Total of pending jobs {_jobs.Count}");
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
                                Console.WriteLine("Job executed successfully.");
                                jobsToRemove.Add(job);
                                break;
                            }
                            catch (System.Exception e)
                            {
                                Console.WriteLine(e.Message);
                                job.Retries--;
                                if (job.Retries > 0)
                                {
                                    await Task.Delay(TimeSpan.FromSeconds(job.DelayAfterRetray), stoppingToken);
                                    Console.WriteLine("The job will try again to succed");
                                }
                            }

                        }
                        if (job.Retries <= 0 && !jobsToRemove.Contains(job))
                        {
                            Console.WriteLine("Jo has exhausted all retries and will be removed.");
                            jobsToRemove.Add(job);
                        }
                    }

                }
                // remove executed jobs
                foreach (var job in jobsToRemove)
                {
                    _jobs.Remove(job);
                }

                Console.WriteLine("Waiting scheduler delay");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }


}
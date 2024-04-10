using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using etlbackgroundworker.Models;

namespace etlbackgroundworker.Services.Core
{
    public interface IJobSchedulerService
    {
        void ScheduleJob(ScheduleJob job);
    }
}
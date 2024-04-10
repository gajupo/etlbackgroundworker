using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace etlbackgroundworker.Models
{
    public class ScheduleJob
    {
        public DateTime NextRunTime { get; set; }
        public Func<CancellationToken, Task> Task { get; set; }
        public int Priority { get; set; }
        public int Retries { get; set; } = 1;

        public int DelayAfterRetray { get; set; }
    }
}
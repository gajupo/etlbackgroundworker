using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using etlbackgroundworker.Services.Core;

namespace etlbackgroundworker.Services
{
    public class MyNotificationHandler : INotificationHandler
    {
        public Task HandleNotificationAsync()
        {
            return Task.CompletedTask;
        }
    }
}
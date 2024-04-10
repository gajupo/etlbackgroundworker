using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace etlbackgroundworker.Services.Core
{
    public interface INotificationHandler
    {
        Task HandleNotificationAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using etlbackgroundworker.Services.Core;

namespace etlbackgroundworker.Services
{
    public class MyDataProcessor : IDataProcessor
    {
        public Task ProcessAsync()
        {
            Console.WriteLine("Data processed...");
            return Task.CompletedTask;
        }
    }
}
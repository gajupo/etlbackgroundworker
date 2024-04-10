using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using etlbackgroundworker.Models;
using Microsoft.Extensions.Hosting;

namespace etlbackgroundworker.Services
{
    public class NotificationQueuedService : BackgroundService
    {
        private readonly ConcurrentQueue<Notification> _notifications = new ConcurrentQueue<Notification>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void EnqueueNotification(Notification notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            _notifications.Enqueue(notification);
            _signal.Release();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _signal.WaitAsync(stoppingToken);

                if (_notifications.TryDequeue(out var notification))
                {
                    Console.WriteLine($"Sending notification to {notification.UserId}: Message {notification.Message}");
                }
            }
        }
    }
}
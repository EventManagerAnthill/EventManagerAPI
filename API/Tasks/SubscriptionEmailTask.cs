using API.Tasks.Scheduling;
using AutoMapper;

using Microsoft.Extensions.DependencyInjection;
using Study.EventManager.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace API.Tasks
{
    public class SubscriptionEmailTask : IScheduledTask
    {
        public string Schedule => "00 12 * * *";

        IServiceScopeFactory _scopeFactory;
        public SubscriptionEmailTask(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var _schedulerService = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISchedulerService>())
            {
                await _schedulerService.SubscriptionEmail();
            }
        }
    }
}

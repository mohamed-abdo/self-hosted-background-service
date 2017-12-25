using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundHostedServices
{
    public class MessagesServiceHost : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // task start executing ....
            return Task.Delay(10000);
        }
    }
}

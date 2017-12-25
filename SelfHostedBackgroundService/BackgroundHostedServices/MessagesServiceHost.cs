using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundHostedServices
{
    public class MessagesServiceHost : BackgroundService
    {
        private readonly ILogger logger;
        public MessagesServiceHost(ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger<MessagesServiceHost>();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // task start executing ....
            var factory = new ConnectionFactory() { HostName = "messagesquare" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                  exchange: "logs",
                                  routingKey: "");

                logger.LogInformation(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    logger.LogInformation(" [x] receiving: {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                logger.LogInformation(" executing ....");
            }
            return Task.CompletedTask;
        }
    }
}

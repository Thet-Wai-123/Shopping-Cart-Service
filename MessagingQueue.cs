using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Redis.OM;
using Redis.OM.Contracts;
using ShoppingCartService.DTOs;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;

namespace ShoppingCartService
{
    public class MessagingQueue
    {
        private IRedisConnection _db_context;

        public MessagingQueue(IRedisConnection connection)
        {
            _db_context = connection;
        }

        public void StartMessagingQueue()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(
                queue: "ShoppingCart_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.Headers.TryGetValue("Action", out var actionObj))
                {
                    string action = Encoding.UTF8.GetString(actionObj as byte[]);
                    await HandleQueueTask(action, message);
                }
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: "ShoppingCart_queue", consumer: consumer);
        }

        private async Task HandleQueueTask(string action, string messageInString)
        {
            if (action == "CREATE")
            {
                var productToAdd = JsonSerializer.Deserialize<AddToShoppingCartDTO>(
                    messageInString
                );
                var newOrder = productToAdd.MapToOrder();
                newOrder.OrderId = CustomKeyGenerator.GenerateKey(newOrder.UserId, newOrder.ItemId);
                var key = await _db_context.SetAsync(newOrder);
            }
            else if (action == "DELETE")
            {
                var productToDelete = JsonSerializer.Deserialize<RemoveFromShoppingCartDTO>(
                    messageInString
                );
                await _db_context.UnlinkAsync(
                    "ShoppingCartService.Models.Order:"
                        + CustomKeyGenerator.GenerateKey(
                            productToDelete.PostedBy,
                            productToDelete.ItemId
                        )
                );
            }
        }
    }
}

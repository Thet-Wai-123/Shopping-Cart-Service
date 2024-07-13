using Redis.OM;
using ShoppingCartService;
using ShoppingCartService.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var provider = new RedisConnectionProvider("redis://localhost:6379");
var connection = provider.Connection;

var messagingQueue = new MessagingQueue(connection);
messagingQueue.StartMessagingQueue();

//A bit bad code to just create an index just to be able to query the data
await provider.Connection.CreateIndexAsync(typeof(Order));

app.MapGet(
    "/{id}",
    async (int id) =>
    {
        var results = new List<int>();
        var allOrders = provider.RedisCollection<Order>();
        await foreach (var item in allOrders.Where(x => x.UserId == id))
        {
            results.Add(item.ItemId);
        }
        return Results.Ok(results);
    }
);

app.Run();

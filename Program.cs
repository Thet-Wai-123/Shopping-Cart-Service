using Redis.OM;
using ShoppingCartService.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var provider = new RedisConnectionProvider("redis://localhost:6379");
var connection = provider.Connection;

connection.CreateIndex(typeof(OrderList));

var orderList = new OrderList { UserId = 1, ItemId = 1, };

var allOrders = provider.RedisCollection<OrderList>();

app.MapGet(
    "/",
    async (int userId) =>
    {
        var results = new List<int>();
        await foreach (var item in allOrders.Where(x => x.UserId == userId))
        {
            results.Add(item.ItemId);
        }
        return Results.Ok(results);
    }
);

//Actually I'm using a messaging queue for this!
//app.MapPost(
//    "/",
//    async (OrderList order) =>
//    {
//        await connection.SetAsync(order);
//        return Results.Ok();
//    }
//);

//app.Run();

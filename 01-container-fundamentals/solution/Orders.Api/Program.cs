using Orders.Api.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// In-memory order collection
var orders = new List<Order>
{
    new() {
        Id = 1,
        CustomerName = "John Smith",
        TotalAmount = 1250.00m,
        Status = "Pending"
    },
    new() {
        Id = 2,
        CustomerName = "Sarah Johnson",
        TotalAmount = 2500.00m,
        Status = "Completed"
    }
};

// GET: Retrieve all orders
app.MapGet("/api/orders", () =>
{
    return Results.Ok(orders);
});

// GET: Retrieve an order by ID
app.MapGet("/api/orders/{id:int}", (int id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);

    return order is not null
        ? Results.Ok(order)
        : Results.NotFound();
});

// POST: Create a new order
app.MapPost("/api/orders", (Order newOrder) =>
{
    newOrder.Id = orders.Count > 0
        ? orders.Max(o => o.Id) + 1
        : 1;

    newOrder.CreatedAt = DateTime.UtcNow;

    orders.Add(newOrder);

    return Results.Created(
        $"/api/orders/{newOrder.Id}",
        newOrder);
});

// PUT: Update an existing order
app.MapPut("/api/orders/{id:int}", (int id, Order updatedOrder) =>
{
    var existingOrder = orders.FirstOrDefault(o => o.Id == id);

    if (existingOrder is null)
        return Results.NotFound();

    existingOrder.CustomerName = updatedOrder.CustomerName;
    existingOrder.TotalAmount = updatedOrder.TotalAmount;
    existingOrder.Status = updatedOrder.Status;

    return Results.Ok(existingOrder);
});

// DELETE: Delete an order
app.MapDelete("/api/orders/{id:int}", (int id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);

    if (order is null)
        return Results.NotFound();

    orders.Remove(order);

    return Results.NoContent();
});

app.Run();
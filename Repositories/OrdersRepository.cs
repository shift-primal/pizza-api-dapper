using Dapper;
using Microsoft.Data.Sqlite;

public class OrdersRepository(string connectionString)
{
    private readonly string _connectionString = connectionString;

    public async Task<List<Order>> GetAll()
    {
        using var conn = new SqliteConnection(_connectionString);

        List<Order> orders = [.. await conn.QueryAsync<Order>("SELECT * FROM orders")];

        if (orders.Count == 0)
            return orders;

        var orderIds = orders.Select(o => o.Id).ToList();

        List<Pizza> pizzas =
        [
            .. await conn.QueryAsync<Pizza>(
                "SELECT * FROM pizzas WHERE orderid IN @OrderIds",
                new { OrderIds = orderIds }
            ),
        ];

        foreach (var order in orders)
            order.Pizzas = [.. pizzas.Where(p => p.OrderId == order.Id)];

        return orders;
    }

    public async Task<Order?> GetById(int id)
    {
        using var conn = new SqliteConnection(_connectionString);

        var order = await conn.QuerySingleOrDefaultAsync<Order>(
            "SELECT * FROM orders WHERE id = @Id",
            new { Id = id }
        );

        if (order != null)
        {
            List<Pizza> pizzas =
            [
                .. await conn.QueryAsync<Pizza>(
                    "SELECT * FROM pizzas WHERE orderid = @OrderId",
                    new { OrderId = order.Id }
                ),
            ];

            order.Pizzas = pizzas;

            return order;
        }

        return null;
    }

    public async Task Create(Order order)
    {
        using var conn = new SqliteConnection(_connectionString);

        await conn.ExecuteAsync(
            @"INSERT INTO orders (customerid, date, total) VALUES (@CustomerId, @Date, @Total)",
            order
        );
    }

    public async Task Update(Order order)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.ExecuteAsync(
            "UPDATE orders SET customerid = @CustomerId, date = @Date, total = @Total WHERE id = @Id",
            order
        );
    }

    public async Task Delete(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.ExecuteAsync("DELETE FROM orders WHERE id = @Id", new { Id = id });
    }
}

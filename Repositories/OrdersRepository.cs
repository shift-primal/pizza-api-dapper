using Dapper;
using Microsoft.Data.Sqlite;

public class OrdersRepository(string connectionString)
{
    private readonly string _connectionString = connectionString;

    public async Task<List<Order>> GetAll()
    {
        using var conn = new SqliteConnection(_connectionString);
        return [.. await conn.QueryAsync<Order>("SELECT * FROM orders")];
    }

    public async Task<Order?> GetById(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<Order>(
            "SELECT * FROM orders WHERE id = @Id",
            new { Id = id }
        );
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

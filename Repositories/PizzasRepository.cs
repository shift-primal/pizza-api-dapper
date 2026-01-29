using Dapper;
using Microsoft.Data.Sqlite;

public class PizzasRepository(string connectionString)
{
    private readonly string _connectionString = connectionString;

    public async Task<List<Pizza>> GetAll()
    {
        using var conn = new SqliteConnection(_connectionString);
        return [.. await conn.QueryAsync<Pizza>("SELECT * FROM pizzas")];
    }

    public async Task<Pizza?> GetById(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<Pizza>(
            "SELECT * FROM pizzas WHERE id = @Id",
            new { Id = id }
        );
    }

    public async Task Create(Pizza pizza)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.ExecuteAsync(
            @"INSERT INTO pizzas (orderid, sizeid) VALUES (@OrderId, @SizeId)",
            pizza
        );
    }

    public async Task Update(Pizza pizza)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.ExecuteAsync(
            "UPDATE pizzas SET name = orderid = @OrderId, sizeid = @SizeId WHERE id = @Id",
            pizza
        );
    }

    public async Task Delete(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.ExecuteAsync("DELETE FROM pizzas WHERE id = @Id", new { Id = id });
    }
}

using Dapper;
using Microsoft.Data.Sqlite;

public class CustomersRepository(string connectionString)
{
    private readonly string _connectionString = connectionString;

    public async Task<List<Customer>> GetAll()
    {
        using var conn = new SqliteConnection(_connectionString);
        return [.. await conn.QueryAsync<Customer>("SELECT * FROM customers")];
    }

    public async Task<Customer?> GetById(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<Customer>(
            "SELECT * FROM customers WHERE id = @Id",
            new { Id = id }
        );
    }

    public async Task Create(Customer customer)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.ExecuteAsync(
            @"INSERT INTO customers (name, email, phone) VALUES (@Name, @Email, @Phone)",
            customer
        );
    }

    public async Task Update(Customer customer)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.ExecuteAsync(
            "UPDATE customers SET name = @Name, email = @Email, phone = @Phone WHERE id = @Id",
            customer
        );
    }

    public async Task Delete(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.ExecuteAsync("DELETE FROM customers WHERE id = @Id", new { Id = id });
    }
}

using Dapper;
using Microsoft.Data.Sqlite;

public class CustomersRepository(string connectionString)
{
    private readonly string _connectionString = connectionString;

    public async Task<List<Customer>> GetAll()
    {
        using var conn = new SqliteConnection(_connectionString);

        List<Customer> customers = [.. await conn.QueryAsync<Customer>("SELECT * FROM customers")];

        if (customers.Count == 0)
            return customers;

        var customerIds = customers.Select(c => c.Id).ToList();

        List<Order> orders =
        [
            .. await conn.QueryAsync<Order>(
                "SELECT * FROM orders WHERE customerid IN @CustomerIds",
                new { CustomerIds = customerIds }
            ),
        ];

        foreach (var customer in customers)
            customer.Orders = [.. orders.Where(o => o.CustomerId == customer.Id)];

        return customers;
    }

    public async Task<Customer?> GetById(int id)
    {
        using var conn = new SqliteConnection(_connectionString);

        var customer = await conn.QuerySingleOrDefaultAsync<Customer>(
            "SELECT * FROM customers WHERE id = @Id",
            new { Id = id }
        );

        if (customer != null)
        {
            List<Order> orders =
            [
                .. await conn.QueryAsync<Order>(
                    "SELECT * FROM orders WHERE customerid = @CustomerId",
                    new { CustomerId = customer.Id }
                ),
            ];

            customer.Orders = orders;

            return customer;
        }

        return null;
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

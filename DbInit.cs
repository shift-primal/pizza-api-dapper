using Dapper;
using Microsoft.Data.Sqlite;

public static class DbInit
{
    public static void Init(string connectionString)
    {
        //language=sql
        var sqlInit =
            @"
        CREATE TABLE IF NOT EXISTS customers (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            email TEXT NOT NULL,
            phone TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS sizes (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            diameter INTEGER NOT NULL,
            price REAL NOT NULL
        );

        CREATE TABLE IF NOT EXISTS toppings (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            price REAL NOT NULL
        );

        CREATE TABLE IF NOT EXISTS orders (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            customerid INTEGER NOT NULL,
            date TEXT NOT NULL,
            total REAL NOT NULL,
            FOREIGN KEY (customerid) REFERENCES customers (id)
        );

        CREATE TABLE IF NOT EXISTS pizzas (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            orderid INTEGER NOT NULL,
            sizeid INTEGER NOT NULL,
            FOREIGN KEY (orderid) REFERENCES orders (id),
            FOREIGN KEY (sizeid) REFERENCES sizes (id)
        );


        CREATE TABLE IF NOT EXISTS pizzatopping (
            pizzaid INTEGER NOT NULL,
            toppingid INTEGER NOT NULL,
            PRIMARY KEY (pizzaid, toppingid),
            FOREIGN KEY (pizzaid) REFERENCES pizza (id),
            FOREIGN KEY (toppingid) REFERENCES toppings (id)
        );
        ";

        //language=sql
        var sqlSeedData =
            @"
        INSERT OR IGNORE INTO sizes (id, name, diameter, price) VALUES
        (1, 'Liten', 24, 80.00),
        (2, 'Medium', 33, 120.00),
        (3, 'Stor', 40, 180.00);

        INSERT OR IGNORE INTO toppings (id, name, price) VALUES
        (1, 'Pepperoni', 20.00),
        (2, 'Skinke', 20.00),
        (3, 'Spekeskinke', 30.00),
        (4, 'Sopp', 10.00),
        (5, 'Mais', 10.00),
        (6, 'Paprika', 10.00),
        (7, 'Kaviar', 100.00),
        (8, 'Safran', 500.00);
          ";

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        connection.ExecuteAsync(sqlInit);
        connection.ExecuteAsync(sqlSeedData);
    }
}

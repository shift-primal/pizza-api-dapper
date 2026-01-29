var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")!;

builder.Services.AddScoped<SizeRepository>(_ => new SizeRepository(connectionString));

var app = builder.Build();

DbInit.Init(connectionString);

app.MapGet("/sizes", async (SizeRepository repo) => repo.GetAll());

app.Run();

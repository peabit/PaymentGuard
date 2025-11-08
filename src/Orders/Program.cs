var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapControllers();

app.Run();
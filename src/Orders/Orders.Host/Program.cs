using Orders;

var appBuilder = WebApplication.CreateBuilder(args);

appBuilder.Services.AddHostedService<Consumer>();

appBuilder.Build().Run();

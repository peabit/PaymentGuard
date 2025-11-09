using Orders;

var appBuilder = WebApplication.CreateBuilder(args);

var bootstrapServers = appBuilder.Configuration
    .GetSection("Kafka:BootstrapServers")
    .Get<string>()!;

appBuilder.Services.AddHostedService(_ => new Consumer(bootstrapServers));

appBuilder.Build().Run();

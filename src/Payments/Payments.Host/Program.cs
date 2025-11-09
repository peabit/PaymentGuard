using Payments;

var appBuilder = WebApplication.CreateBuilder(args);

var bootstrapServers = appBuilder.Configuration
    .GetSection("Kafka:BootstrapServers")
    .Get<string>()!;

appBuilder.Services
    .AddSingleton<PaymentEventRepository>()
    .AddSingleton(new PaymentEventPublisher(bootstrapServers))
    .AddHostedService<PaymentEventGenerator>();

appBuilder.Build().Run();
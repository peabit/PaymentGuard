using Payments;

var appBuilder = WebApplication.CreateBuilder(args);

appBuilder.Services
    .AddSingleton<PaymentEventRepository>()
    .AddSingleton<PaymentEventPublisher>()
    .AddHostedService<PaymentEventGenerator>();

appBuilder.Build().Run();
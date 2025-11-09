using Host;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddHostedService<KafkaListener>();
services.AddControllers();
services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.Run();

// Host
//    .AddPresentation()
//    .AddApplication()
//    .AddInfrastructure()

// PaymentGuard.Presentation
//    -> Endpoints
//    -> Consumers
//    -> Jobs
//    -> Composer
// PaymentGuard.Application
//    -> + GetBadPaymentsQuery.Execute(GetBadPaymentsQueryArgs args)
//    -> + GetBadPaymentsCountQuery.Execute(GetBadPaymentsCountQueryArgs args)
//    -> + RefreshBadPaymentsCountMetricCommand.Execute()
//    -> + SavePaymentCompletedEventCommand.Execute()
//    -> + SaveOrderPaidEventCommand.Execute()
//    -> + CleanAgedEventCommand.Execute()
// Infrastructure
//    -> PaymentGuard.Infrastructure.Kafka
//    -> PaymentGuard.Infrastructure.Postgres
//    -> PaymentGuard.Infrastructure.Prometheus
//    -> PaymentGuard.Infrastructure.Jobs

// AddJob<...>(TimeSpan ...) Jobs: {}
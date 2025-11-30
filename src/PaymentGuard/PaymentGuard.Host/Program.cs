using PaymentGuard.Infrastructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddKafka(typeof(Program).Assembly);

var app = builder.Build();

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
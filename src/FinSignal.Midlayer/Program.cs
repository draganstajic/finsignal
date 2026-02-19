using FinSignal.Midlayer.Audit;
using FinSignal.Midlayer.Reconciliation;
using FinSignal.Midlayer.EventBus;
using FinSignal.Midlayer.Signals;
using FinSignal.Midlayer.Data;
using Microsoft.EntityFrameworkCore;
using FinSignal.Midlayer.Infrastructure;
using FinSignal.Midlayer.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// MVC / API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// FinSignal core services
builder.Services.AddSingleton<IAuditStore, InMemoryAuditStore>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// Signal service registration
//builder.Services.AddSingleton<InvoiceStatusStore>();
builder.Services.AddScoped<SignalProcessor>();

//registrujem repository
builder.Services.AddScoped<IInvoiceStatusRepository, EfInvoiceStatusRepository>();

//DB PGSQL
builder.Services.AddDbContext<FinSignalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Reconciliation services
builder.Services.AddSingleton<InMemoryLedger>();
builder.Services.AddSingleton<MatchingEngine>();

//Registration of EventStoreRepository
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
// Add DbContext drajver za komunikaciju sa db
/*builder.Services.AddDbContext<FinSignalDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")
    )
);*/
// Registration of RabbitMQ connection factory


builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMQ"));
    builder.Services.AddSingleton<RabbitMqPublisher>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); ne zaboraviti da otkomentarišeš kada ide sa lokala na prod.
app.UseAuthorization();
app.MapControllers();

app.Run();


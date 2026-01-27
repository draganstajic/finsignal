using FinSignal.Midlayer.Audit;
using FinSignal.Midlayer.Reconciliation;
using FinSignal.Midlayer.EventBus;

var builder = WebApplication.CreateBuilder(args);

// MVC / API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FinSignal core services
builder.Services.AddSingleton<IAuditStore, InMemoryAuditStore>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// Reconciliation services
builder.Services.AddSingleton<InMemoryLedger>();
builder.Services.AddSingleton<MatchingEngine>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


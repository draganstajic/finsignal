# FinSignal Copilot Instructions

## Project Overview
FinSignal is an **event-driven fintech midlayer** for invoice and payment workflows. Built on ASP.NET Core 8 with minimal dependencies, it emphasizes auditability through correlation tracking and AI-assisted system analysis.

**Key Focus Areas:**
- Invoice ingestion via webhook endpoints
- Event-driven architecture with internal event propagation
- Complete audit trail with correlation IDs
- Extensible for future payment workflow integration

## Architecture & Data Flow

### Event-Driven Core
The system is built around a **correlation ID** pattern for tracking transactions:

1. **Invoice Ingestion** → `InvoiceController.Receive()` generates a unique `correlationId`
2. **Event Publishing** → `IEventBus.Publish()` routes events to `InMemoryAuditStore`
3. **Audit Trail** → All events indexed by `correlationId` for retrieval

**Key Files:**
- [src/FinSignal.Midlayer/Events/BaseEvent.cs](src/FinSignal.Midlayer/Events/BaseEvent.cs) - Core event abstraction with `CorrelationId`, `EventType`, `Source`, `OccurredAt`
- [src/FinSignal.Midlayer/EventBus/InMemoryEventBus.cs](src/FinSignal.Midlayer/EventBus/InMemoryEventBus.cs) - Event routing (currently simple passthrough to audit)

### Component Boundaries

| Component | Purpose | Implementation |
|-----------|---------|-----------------|
| **Controllers** | HTTP endpoints for invoice ingestion & audit queries | [InvoiceController](src/FinSignal.Midlayer/Controllers/InvoiceController.cs), [AuditController](src/FinSignal.Midlayer/Controllers/AuditController.cs) |
| **EventBus** | Interface abstraction for event publishing | `IEventBus` (in-memory for now, designed for replacement) |
| **AuditStore** | Persistent event log indexed by correlation ID | `IAuditStore` with in-memory implementation |
| **Events** | Event definitions with payload | Inherit from `BaseEvent` |
| **Models** | DTOs for API contracts | `InvoiceDto` |

## Development Patterns

### Adding New Event Types
1. Create class in `Events/` inheriting from `BaseEvent`
2. Add payload properties (marked with `init` for immutability)
3. Example: [InvoiceReceivedEvent.cs](src/FinSignal.Midlayer/Events/InvoiceReceivedEvent.cs)

```csharp
public class MyNewEvent : BaseEvent
{
    public string MyData { get; init; } = default!;
}
```

### Adding New Endpoints
1. Create controller in `Controllers/` inheriting from `ControllerBase`
2. Inject dependencies via constructor (e.g., `IEventBus`, `IAuditStore`)
3. Use `[HttpPost]` and `[Route("api/...")]` attributes
4. Emit events through `_eventBus.Publish()` for audit trails

### Dependency Injection
All services registered in [Program.cs](src/FinSignal.Midlayer/Program.cs) as singletons:
- `IAuditStore` → `InMemoryAuditStore`
- `IEventBus` → `InMemoryEventBus`

Extension point: To replace implementations, update these registrations without touching business logic.

## Build & Run

**Prerequisites:** .NET 8 SDK

**Build:**
```bash
dotnet build src/FinSignal.sln
```

**Run:**
```bash
dotnet run --project src/FinSignal.Midlayer/FinSignal.Midlayer.csproj
```

**API Documentation:** Swagger/OpenAPI at `/swagger` when running in Development

## Critical Integration Points

- **Correlation ID Strategy:** Every transaction must have a unique `correlationId` for tracing. Generated at entry point, propagated through `BaseEvent`.
- **Event Immutability:** Use `init` properties on events to prevent accidental mutation after creation.
- **Audit Idempotency:** Currently in-memory; future persistence layer should handle duplicate event detection via `EventId` (Guid).
- **Invoice Source:** Events capture `Source` field (e.g., "eFaktura"). Use for multi-source invoice reconciliation.

## Project-Specific Conventions

- **Namespace Structure:** `FinSignal.Midlayer.{Feature}` (e.g., `FinSignal.Midlayer.Audit`)
- **API Routes:** RESTful under `api/` prefix (e.g., `/api/invoices`, `/api/audit`)
- **Nullable Reference Types:** Enabled in .csproj; use `default!` for non-nullable DTOs
- **Implicit Usings:** Enabled; standard .NET namespaces auto-imported
- **Init-Only Properties:** Preferred for DTOs and events to enforce immutability

## Future Extension Points

1. **Event Subscribers:** Replace in-memory event bus with pub/sub (e.g., RabbitMQ, Azure Service Bus)
2. **Persistent Audit Store:** Implement `IAuditStore` with SQL/NoSQL backend
3. **Payment Workflows:** New event types and controllers following the same pattern
4. **AI Analysis Integration:** Audit trail provides structured data for anomaly detection and pattern recognition

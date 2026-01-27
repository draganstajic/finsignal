using FinSignal.Midlayer.Audit;
using Microsoft.AspNetCore.Mvc;

namespace FinSignal.Midlayer.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly IAuditStore _audit;

    public AuditController(IAuditStore audit)
    {
        _audit = audit;
    }

    [HttpGet("{correlationId}")]
    public IActionResult Get(string correlationId)
    {
        return Ok(_audit.GetByCorrelationId(correlationId));
    }
}

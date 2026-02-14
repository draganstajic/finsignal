using FinSignal.Midlayer.Models;
using FinSignal.Midlayer.Signals;
using Microsoft.AspNetCore.Mvc;

namespace FinSignal.Midlayer.Controllers;

[ApiController]
[Route("signals")]
public class SignalsController : ControllerBase
{
    private readonly IInvoiceStatusRepository _repo;

    public SignalsController(IInvoiceStatusRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("invoices")]
    public async Task<ActionResult<IEnumerable<InvoiceHealthDto>>> GetAll()
    {
        var all = await _repo.GetAllAsync();
        var result = all.Select(x => new InvoiceHealthDto
        {
            InvoiceNumber = x.InvoiceNumber,
            InvoiceAmount = x.InvoiceAmount,
            PaidAmount = x.PaidAmount,
            RemainingAmount = x.Remaining,
            State = x.State.ToString()
        });

        return Ok(result);
    }

    [HttpGet("invoices/{invoiceNumber}")]
    public async Task<ActionResult<InvoiceHealthDto>> GetOne(string invoiceNumber)
    {
        var status = await _repo.GetAsync(invoiceNumber);

        if (status == null)
            return NotFound();

        return Ok(new InvoiceHealthDto
        {
            InvoiceNumber = status.InvoiceNumber,
            InvoiceAmount = status.InvoiceAmount,
            PaidAmount = status.PaidAmount,
            RemainingAmount = status.Remaining,
            State = status.State.ToString()
        });
    }
}
using FinSignal.Midlayer.Models;
using FinSignal.Midlayer.Signals;
using Microsoft.AspNetCore.Mvc;

namespace FinSignal.Midlayer.Controllers;

[ApiController]
[Route("signals")]
public class SignalsController : ControllerBase
{
    private readonly InvoiceStatusStore _store;

    public SignalsController(InvoiceStatusStore store)
    {
        _store = store;
    }

    [HttpGet("invoices")]
    public ActionResult<IEnumerable<InvoiceHealthDto>> GetAll()
    {
        var result = _store.All().Select(x => new InvoiceHealthDto
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
    public ActionResult<InvoiceHealthDto> GetOne(string invoiceNumber)
    {
        var status = _store.Get(invoiceNumber);

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
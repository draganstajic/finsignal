using FinSignal.Midlayer.Data;
using FinSignal.Midlayer.Signals;
using Microsoft.EntityFrameworkCore;

namespace FinSignal.Midlayer.Infrastructure;

public class EfInvoiceStatusRepository : IInvoiceStatusRepository
{
private readonly FinSignalDbContext _db;

public EfInvoiceStatusRepository(FinSignalDbContext db)
{
_db = db;
}

public async Task<InvoiceStatus?> GetAsync(string invoiceNumber)
{
return await _db.InvoiceStatuses
.SingleOrDefaultAsync(x => x.InvoiceNumber == invoiceNumber);
}

public async Task<List<InvoiceStatus>> GetAllAsync()
{
return await _db.InvoiceStatuses.ToListAsync();
}

public async Task SaveAsync(InvoiceStatus status)
{
var existing = await _db.InvoiceStatuses
.AsNoTracking()
.SingleOrDefaultAsync(x => x.InvoiceNumber == status.InvoiceNumber);

if (existing == null)
_db.InvoiceStatuses.Add(status);
else
_db.InvoiceStatuses.Update(status);

await _db.SaveChangesAsync();
}
}

using FinSignal.Midlayer.Signals;

public interface IInvoiceStatusRepository
{
    Task<InvoiceStatus?> GetAsync(string invoiceNumber);
    Task<List<InvoiceStatus>> GetAllAsync();
 //   Task AddAsync(InvoiceStatus status);
    Task SaveAsync(InvoiceStatus status);
}
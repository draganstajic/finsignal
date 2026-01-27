namespace FinSignal.Midlayer.Models;

public class InvoiceDto
{
    public string InvoiceNumber { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RSD";
    public string Supplier { get; set; } = default!;
}
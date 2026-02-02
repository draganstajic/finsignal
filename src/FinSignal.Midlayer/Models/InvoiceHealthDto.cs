namespace FinSignal.Midlayer.Models;
public class InvoiceHealthDto
{
    public string InvoiceNumber { get; set; } = default!;
    public decimal InvoiceAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string State { get; set; } = default!;
}
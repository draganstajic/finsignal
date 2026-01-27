namespace FinSignal.Midlayer.Models;

public class PaymentDto
{
    public string PaymentReference { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RSD";
    public DateTime PaidAt { get; set; }
}
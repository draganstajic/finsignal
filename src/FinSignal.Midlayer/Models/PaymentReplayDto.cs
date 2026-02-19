namespace FinSignal.Midlayer.Models;

public class PaymentReplayDto
{
    public string Reference { get; set; } = default!;
    public decimal Amount { get; set; }
}
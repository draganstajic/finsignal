namespace FinSignal.Midlayer.Signals;
/*public class InvoiceStatus
{
    public string InvoiceNumber { get; }
    public decimal InvoiceAmount { get; }
    public decimal PaidAmount { get; private set; }

    public decimal Remaining => InvoiceAmount - PaidAmount;
    public InvoiceState State { get; private set; } = InvoiceState.Open;

    public InvoiceStatus(string invoiceNumber, decimal invoiceAmount)
    {
        InvoiceNumber = invoiceNumber;
        InvoiceAmount = invoiceAmount;
    }

    public void ApplyPayment(decimal amount)
    {
        PaidAmount += amount;

        if (PaidAmount <= 0)
            State = InvoiceState.Open;
        else if (PaidAmount < InvoiceAmount)
            State = InvoiceState.PartiallyPaid;
        else if (PaidAmount == InvoiceAmount)
            State = InvoiceState.Paid;
        else
            State = InvoiceState.Overpaid;
    }
} */
public class InvoiceStatus
{
    public string InvoiceNumber { get; private set; } = default!;
    public decimal InvoiceAmount { get; private set; }
    public decimal PaidAmount { get; private set; }
    public InvoiceState State { get; private set; } = InvoiceState.Open;
    public decimal Remaining => InvoiceAmount - PaidAmount;
    // EF Core ONLY
    private InvoiceStatus() { }

    // Domain constructor
    public InvoiceStatus(string invoiceNumber, decimal invoiceAmount)
    {
        InvoiceNumber = invoiceNumber;
        InvoiceAmount = invoiceAmount;
        PaidAmount = 0;
        State = InvoiceState.Open;
    }

    public void ApplyPayment(decimal amount)
    {
        PaidAmount += amount;

        if (PaidAmount <= 0)
            State = InvoiceState.Open;
        else if (PaidAmount < InvoiceAmount)
            State = InvoiceState.PartiallyPaid;
        else if (PaidAmount == InvoiceAmount)
            State = InvoiceState.Paid;
        else
            State = InvoiceState.Overpaid;
    }
}

namespace FinSignal.Midlayer.Infrastructure.Messaging;

public class RabbitMqSettings
{
    public string Host { get; set; } = default!;
    public int Port { get; set; }
    public string User { get; set; } = default!;
    public string Password { get; set; } = default!;
}
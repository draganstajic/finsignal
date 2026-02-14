using Microsoft.EntityFrameworkCore;
using FinSignal.Midlayer.Signals;

namespace FinSignal.Midlayer.Data;

public class FinSignalDbContext : DbContext
{
    public FinSignalDbContext(DbContextOptions<FinSignalDbContext> options)
        : base(options)
    {
    }

    public DbSet<InvoiceStatus> InvoiceStatuses => Set<InvoiceStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvoiceStatus>()
            .HasKey(x => x.InvoiceNumber);
    }
}

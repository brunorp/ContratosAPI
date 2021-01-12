using ContratosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContratosAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Contrato>().HasMany(contrato => contrato.Prestacoes)
            .WithOne(prestacao => prestacao.Contrato);
        }

        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Prestacao> Prestacoes { get; set; }
    }
}
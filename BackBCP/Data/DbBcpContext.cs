using BackBCP.Models;
using Microsoft.EntityFrameworkCore;

namespace BackBCP.Data
{
    public class DbBcpContext:DbContext
    {
        public DbBcpContext(DbContextOptions<DbBcpContext> options): base(options)
        {

        }

        public DbSet<Usuario> usuario { get; set; }
        public DbSet<TipoMoneda> tipoMoneda { get; set; }
        public DbSet<TipoCambio> tipoCambio { get; set; }
    }
}

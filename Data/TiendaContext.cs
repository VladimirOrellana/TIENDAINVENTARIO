using Microsoft.EntityFrameworkCore;
using TiendaInventario.Models;
namespace TiendaInventario.Data
{
    public class TiendaContext : DbContext
    {
        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using TiendaInventario.Data;
using TiendaInventario.ViewModels;
using System.Linq;

namespace TiendaInventario.Controllers
{
    public class EstadisticasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadisticasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var productosConMenorStock = _context.Productos
                .OrderBy(p => p.Cantidad)
                .Take(5)
                .ToList();

            var valorTotalInventario = _context.Productos
                .Sum(p => p.Precio * p.Cantidad);

            var modelo = new EstadisticasViewModel
            {
                ProductosConMenorStock = productosConMenorStock,
                ValorTotalInventario = valorTotalInventario
            };

            return View(modelo);
        }
    }
}

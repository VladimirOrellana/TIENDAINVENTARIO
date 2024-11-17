using TiendaInventario.Models;

namespace TiendaInventario.ViewModels
{
    public class EstadisticasViewModel
    {
        public List<Producto> ProductosConMenorStock { get; set; }
        public decimal ValorTotalInventario { get; set; }
    }
}


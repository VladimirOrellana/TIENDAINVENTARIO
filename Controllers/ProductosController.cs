using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // Asegúrate de agregar esta referencia
using TiendaInventario.Data;
using TiendaInventario.Models;

namespace TiendaInventario.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        [Authorize] // Solo usuarios autenticados pueden acceder
        public async Task<IActionResult> Index(string searchString, decimal? minPrice, decimal? maxPrice, int? minQuantity, int? maxQuantity)
        {
            var productos = from p in _context.Productos
                            select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Nombre.Contains(searchString) || p.Descripcion.Contains(searchString));
            }

            if (minPrice.HasValue)
            {
                productos = productos.Where(p => p.Precio >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                productos = productos.Where(p => p.Precio <= maxPrice.Value);
            }

            if (minQuantity.HasValue)
            {
                productos = productos.Where(p => p.Cantidad >= minQuantity.Value);
            }

            if (maxQuantity.HasValue)
            {
                productos = productos.Where(p => p.Cantidad <= maxQuantity.Value);
            }

            return View(await productos.ToListAsync());
        }

        // GET: Productos/Details/5
        [Authorize] // Cualquier usuario autenticado puede ver detalles
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        [Authorize(Roles = "Admin")] // Solo los administradores pueden crear productos
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Solo los administradores pueden crear productos
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Cantidad")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        [Authorize(Roles = "Admin")] // Solo los administradores pueden editar productos
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Solo los administradores pueden editar productos
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Cantidad")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Vuelve a lanzar la excepción si hay un error en la concurrencia
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Delete/5
        [Authorize(Roles = "Admin")] // Solo los administradores pueden eliminar productos
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Solo los administradores pueden eliminar productos
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockProject.Data;
using StockProject.Models;
using X.PagedList;

namespace StockProject.Controllers
{
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public InventarioController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Obtiene los productos ordenados en paginas, recibe el numero de pagina que debe mostrar
        public async Task<IActionResult> Productos(int? page)
        {

            int pageSize = 18;
            var productList = await _dbContext.Products.ToListAsync();
            var pagedProductList = productList.ToPagedList(page ?? 1, pageSize);
            if (page > pagedProductList.PageCount)
                pagedProductList = productList.ToPagedList(1, pageSize);
            return View(pagedProductList);
        }

        // Devuelve la vista "Detalles.cshtml" dependiendo del id que le entreguen
        public async Task<IActionResult> Detalles(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var producto = await _dbContext.Products.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            var category = await _dbContext.Categorias.ToListAsync();

            // Pasar las categorías y el producto a la vista
            ViewBag.Categorias = new SelectList(category, "Id_categoria", "Nombre_categoria");
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Products producto)
        {
            if (id != producto.Id_producto)
            {
                Console.WriteLine("Error");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Update(producto);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Detalles", new { id = producto.Id_producto });
            }

            // Si el modelo no es válido, regresamos a la misma vista de detalles con los mensajes de error.
            return View("Detalles", producto);
        }
    }
}
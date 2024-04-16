using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockProject.Data;
using X.PagedList;

namespace StockProject.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public EmpleadosController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Obtiene los empleados ordenados en paginas, recibe el numero de pagina que debe mostrar
        public async Task<IActionResult> Empleados(int? page)
        {
            int pageSize = 18;
            var employeetList = await _dbContext.Empleados.ToListAsync();
            var pagedEmployeeList = employeetList.ToPagedList(page ?? 1, pageSize);
            if (page > pagedEmployeeList.PageCount)
                pagedEmployeeList = employeetList.ToPagedList(1, pageSize);
            return View(pagedEmployeeList);
        }
    }
}

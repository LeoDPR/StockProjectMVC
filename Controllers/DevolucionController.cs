using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockProject.Data;
using StockProject.Models;
using System.Data;
using X.PagedList;

namespace StockProject.Controllers
{
    public class DevolucionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public DevolucionController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IActionResult> Devolucion(string? search, int page = 1)
        {
            var productos = await _dbContext.Categorias.ToListAsync();
            ViewBag.Productos = new SelectList(productos, "Nombre_producto");

            if (string.IsNullOrEmpty(search))
            {
                var model = await _dbContext.Asignacion.ToPagedListAsync(page, 15);
                return View(model);
            }
            else
            {
                ViewBag.search = search; //GUARDA LA BUSQUEDA PARA QUE AL USAR LA PAGINACION NO SE PIERDA
                var model = await _dbContext.Asignacion
                .Where(r => search == null || r.Nombre_empleado.StartsWith(search)
                || r.Nombre_empleado.Contains(search) || r.Nombre_producto.StartsWith(search)
                || r.Nombre_producto.Contains(search)).OrderBy(r => r.Nombre_empleado).ToPagedListAsync(page, 15);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerLista(string? search, int page = 1)
        {
            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
            }

            var model = await _dbContext.Asignacion
                .Where(r => search == null || r.Nombre_empleado.StartsWith(search)
                || r.Nombre_empleado.Contains(search) || r.Nombre_producto.StartsWith(search)
                || r.Nombre_producto.Contains(search)).OrderBy(r => r.Nombre_empleado).ToPagedListAsync(page, 15);
            return PartialView("_ListaAsignaciones", model);
        }

        public async Task<IActionResult> ObtenerRegistro(int id, int option)
        {
            var registro = await _dbContext.Asignacion
               .Where(r => r.Id_asignacion == id).FirstOrDefaultAsync();
            // Devolver el registro a la vista en el modal
            if (option == 1)
            {
                return PartialView("_ModalEditarAsignacion", registro);
            }
            else if (option == 3)
            {
                return PartialView("_ModalEliminarAsignacion", registro);
            }
            return View("Devolucion");
        }


        public JsonResult EliminarRegistro(int[] ids, Enum action)
        {
            // elimina los registros de la tabla asignaciones, puede ser uno o varios y actualiza el stock del producto que se devolvio.
            bool result = false;
            bool stockStatus = true;
            string errores = "";
            string mensaje = "";
            try
            {
                foreach (var id in ids)
                {

                    var registro = _dbContext.Asignacion.Find(id);
                    //Si se encuentra el ID de la asignacion
                    if (registro != null)
                    {
                        /*Actualiza el inventario del producto que se regresó*/

                        var queryUpdate = (from r in _dbContext.Products
                                           where r.Nombre_producto == registro.Nombre_producto
                                           select r).FirstOrDefault();
                        //IF que Verifica que se haya encontrado un producto con el mismo nombre de la asignacion
                        if (queryUpdate != null)
                        {

                            queryUpdate.Stock = queryUpdate.Stock + registro.Cantidad;
                        }
                        else
                        {
                            stockStatus = false;
                            //El nombre encontrado en la asignacion no coincide con el nombre registrado en la tabla "products"
                            errores += registro.Nombre_producto + ": " + registro.Cantidad + "\n";
                        }

                        //Elimina el el registro de la asignacion que se regresó de la tabla asignaciones
                        _dbContext.Asignacion.Remove(registro);

                        int rowsAffected = _dbContext.SaveChanges();
                        result = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores de manera adecuada, por ejemplo, registrando el error o devolviendo un mensaje de error.
                Console.WriteLine("Error al eliminar registros: " + ex.Message);
                result = false;
            }
            if (stockStatus)
            {
                mensaje = "Se ha eliminado y agregado a stock correctamente";
            }
            else
            {
                mensaje = "Se ha eliminado la asignacion, agrega manualmente el stock de los siguientes productos: " + errores;
            }
            //return Json(result);
            var data = new
            {
                result,
                mensaje,
                stockStatus
            };
            return Json(data);

        }


        [HttpPost]
        public async Task<IActionResult> UpdateAsignacion(int id, Asignacion asignacion)
        {

            if (id != asignacion.Id_asignacion)
            {
                Console.WriteLine("Error");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Update(asignacion);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Devolucion", new { id = asignacion.Id_asignacion });
            }

            // Si el modelo no es válido, regresamos a la misma vista de detalles con los mensajes de error.
            return View("Devolucion", asignacion);
        }

        [HttpGet]
        public async Task<IActionResult> CrearDevolucion(string nombre)
        {
            ViewBag.nombreEmpleado = nombre;
            //ViewBag.numEmpleado = await _dbContext.Empleados.Where(a => a.Nombre:empleado == db_Context.Asignacion.Nombre_empleado) En esta linea se recuperar el numero de empleado una vez que se haga el modelo de empleado.
            var asignacionesEmpleado = await _dbContext.Asignacion.Where(a => a.Nombre_empleado == nombre).OrderBy(a => a.Nombre_producto).ToListAsync();
            /*if (asignacionesEmpleado.Count != 0)
            {*/
            return View(asignacionesEmpleado);

            //}


        }



    }
}
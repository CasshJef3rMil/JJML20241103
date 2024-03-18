using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJML20241103.Models;

namespace JJML20241103.Controllers
{
    public class EmpleadoesController : Controller
    {
        private readonly JJML20241103BDContext _context;

        public EmpleadoesController(JJML20241103BDContext context)
        {
            _context = context;
        }

        // GET: Empleadoes
        public async Task<IActionResult> Index()
        {
              return _context.Empleados != null ? 
                          View(await _context.Empleados.ToListAsync()) :
                          Problem("Entity set 'JJML20241103BDContext.Empleados'  is null.");
        }

        // GET: Empleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                
                //este es para que se muestre en la vista detalles
                .Include(s=> s.ReferenciasPersonales)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            ViewBag.Accion = "Details";
            return View(empleado);
        }

        // GET: Empleadoes/Create
        public IActionResult Create()
        {
            var empleado = new Empleado();
            empleado.FechaContratacion = DateTime.Now;
            empleado.Edad = 0;
            empleado.ReferenciasPersonales = new List<ReferenciasPersonale>();
            empleado.ReferenciasPersonales.Add(new ReferenciasPersonale
            {
                
            });
            ViewBag.Accion = "Create";
            return View(empleado);
        }

        //public IActionResult Create()
        //{
        //    var empleado = new Empleado();
        //    empleado.FechaContratacion = DateTime.Now;
        //    empleado.Nombre = "";
        //    empleado.Apellido = "";
        //    empleado.Edad = 0;
        //    empleado.Cargo = "";
        //    empleado.ReferenciasPersonales = new List<ReferenciasPersonale>();
        //    empleado.ReferenciasPersonales.Add(new ReferenciasPersonale
        //    {

        //    });

        //    ViewBag.Accion = "Create";
        //    return View(empleado);
        //}

        // POST: Empleadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Edad,Cargo,FechaContratacion,ReferenciasPersonales")] Empleado empleado)
        {

            _context.Add(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

       //metodo agregar detalles
    
        [HttpPost]
        public IActionResult AgregarDetalles([Bind("Id,Nombre,Apellido,Edad,Cargo,FechaContratacion,ReferenciasPersonales")] Empleado empleado, string accion)
        {
            empleado.ReferenciasPersonales.Add(new ReferenciasPersonale { Nombre = "" });
            ViewBag.Accion = accion;
            //esto de aca abajo es fundamental (Accion), para editar
            return View(accion , empleado); // Redirecciona a la vista Create después de agregar los detalles
        }


        public IActionResult EliminarDetalles([Bind("Id,Nombre,Apellido,Edad,Cargo,FechaContratacion,ReferenciasPersonales")] Empleado empleado, int index, string accion)
        {
            {
                var det = empleado.ReferenciasPersonales[index];
                if (accion == "Edit" && det.Id > 0)
                {
                    det.Id = det.Id * -1;
                }
                else
                {
                    empleado.ReferenciasPersonales.RemoveAt(index);
                }

                ViewBag.Accion = accion;
                return View(accion, empleado);
            }
        }

        // GET: Empleadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                 .Include(s => s.ReferenciasPersonales)
                .FirstAsync(s => s.Id == id);
             
            if (empleado == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(empleado);
        }

        // POST: Empleadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( int id, [Bind("Id,Nombre,Apellido,Edad,Cargo,FechaContratacion,ReferenciasPersonales")] Empleado empleado)
        {
            try
            {
                // Obtener los datos de la base de datos que van a ser modificados
                var facturaUpdate = await _context.Empleados
                        .Include(s => s.ReferenciasPersonales)
                        .FirstAsync(s => s.Id == empleado.Id);
                facturaUpdate.Nombre = empleado.Nombre;
                facturaUpdate.Apellido = empleado.Apellido; /*.Where(s => s.Id > -1).Sum(s => s.PrecioUnitario * s.Cantidad);*/
                facturaUpdate.Edad = empleado.Edad;
                facturaUpdate.Cargo = empleado.Cargo;
                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                var detNew = empleado.ReferenciasPersonales.Where(s => s.Id == 0);
                foreach (var d in detNew)
                {
                    facturaUpdate.ReferenciasPersonales.Add(d);
                }
                // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                var detUpdate = empleado.ReferenciasPersonales.Where(s => s.Id > 0);
                foreach (var d in detUpdate)
                {
                    var det = facturaUpdate.ReferenciasPersonales.FirstOrDefault(s => s.Id == d.Id);
                    det.Nombre = d.Nombre;
                    det.Apellido = d.Apellido;
                    det.Relacion = d.Relacion;
                    det.Telefono = d.Telefono;
                }
                // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                var delDet = empleado.ReferenciasPersonales.Where(s => s.Id < 0).ToList();
                if (delDet != null && delDet.Count > 0)
                {
                    foreach (var d in delDet)
                    {
                        d.Id = d.Id * -1;
                        var det = facturaUpdate.ReferenciasPersonales.FirstOrDefault(s => s.Id == d.Id);
                        _context.Remove(det);
                        // facturaUpdate.DetFacturaVenta.Remove(det);
                    }
                }
                // Aplicar esos cambios a la base de datos
                _context.Update(facturaUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(empleado.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Empleadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Delete";
            return View(empleado);
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleados == null)
            {
                return Problem("Entity set 'JJML20241103BDContext.Empleados'  is null.");
            }
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
          return (_context.Empleados?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

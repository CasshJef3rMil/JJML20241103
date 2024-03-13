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
            return View("Create", empleado); // Redirecciona a la vista Create después de agregar los detalles
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

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Edad,Cargo,FechaContratacion")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
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
            return View(empleado);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Data;
using CitasMedicas.Models;

namespace CitasMedicas.Controllers
{
    public class MedicamentosRecetasController : Controller
    {
        private readonly CitasMedicasContext _context;

        public MedicamentosRecetasController(CitasMedicasContext context)
        {
            _context = context;
        }

        // GET: MedicamentosRecetas
        //public async Task<IActionResult> Index()
        //{
        //    var citasMedicasContext = _context.MedicamentoReceta.Include(m => m.Medicamento);
        //    return View(await citasMedicasContext.ToListAsync());
        //}

        // GET: MedicamentosRecetas/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.MedicamentoReceta == null)
        //    {
        //        return NotFound();
        //    }

        //    var medicamentoReceta = await _context.MedicamentoReceta
        //        .Include(m => m.Medicamento)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (medicamentoReceta == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(medicamentoReceta);
        //}

        // GET: MedicamentosRecetas/Create
        public IActionResult Create(int citaId)
        {
            //ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Id");
            ViewData["MedicamentoId"] = new SelectList(
                _context.Medicamentos,
                "Id",
                "NombreMedicamento"
            );
            ViewBag.CitaId = citaId;
            return View();
        }

        // POST: MedicamentosRecetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MedicamentoId,Dosis,Instrucciones,Cantidad")] MedicamentoReceta medicamentoReceta,int citaId)
        {
            medicamentoReceta.CitaId= citaId;
            ModelState.Remove("Cita");
            ModelState.Remove("Medicamento");
            if (ModelState.IsValid)
            {
                _context.Add(medicamentoReceta);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Citas", new { id = citaId });
            }
            ViewData["MedicamentoId"] = new SelectList(
                _context.Medicamentos,
                "Id",
                "NombreMedicamento"
            );
            ViewBag.CitaId = citaId;
            return View();

        }

        // GET: MedicamentosRecetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MedicamentoReceta == null)
            {
                return NotFound();
            }

            var medicamentoReceta = await _context.MedicamentoReceta.FindAsync(id);
            if (medicamentoReceta == null)
            {
                return NotFound();
            }
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Id", medicamentoReceta.MedicamentoId);
            return View(medicamentoReceta);
        }

        // POST: MedicamentosRecetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MedicamentoId,Dosis,Instrucciones,Cantidad")] MedicamentoReceta medicamentoReceta)
        {
            if (id != medicamentoReceta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicamentoReceta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentoRecetaExists(medicamentoReceta.Id))
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
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Id", medicamentoReceta.MedicamentoId);
            return View(medicamentoReceta);
        }

        // GET: MedicamentosRecetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MedicamentoReceta == null)
            {
                return NotFound();
            }

            var medicamentoReceta = await _context.MedicamentoReceta
                .Include(m => m.Medicamento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicamentoReceta == null)
            {
                return NotFound();
            }
            ViewBag.CitaId = medicamentoReceta.CitaId;
            return View(medicamentoReceta);
        }

        // POST: MedicamentosRecetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int citaId=0;
            if (_context.MedicamentoReceta == null)
            {
                return Problem("Entity set 'CitasMedicasContext.MedicamentoReceta'  is null.");
            }
            var medicamentoReceta = await _context.MedicamentoReceta.FindAsync(id);
            if (medicamentoReceta != null)
            {
                citaId = medicamentoReceta.CitaId;
                _context.MedicamentoReceta.Remove(medicamentoReceta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "Citas", new { id = citaId });
        }

        private bool MedicamentoRecetaExists(int id)
        {
          return (_context.MedicamentoReceta?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

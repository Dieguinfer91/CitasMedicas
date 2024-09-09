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
    public class AntecedentesController : Controller
    {
        private readonly CitasMedicasContext _context;

        public AntecedentesController(CitasMedicasContext context)
        {
            _context = context;
        }

        // GET: Antecedentes
        public async Task<IActionResult> Index()
        {
              return _context.Antecedentes != null ? 
                          View(await _context.Antecedentes.ToListAsync()) :
                          Problem("Entity set 'CitasMedicasContext.Antecedentes'  is null.");
        }

        // GET: Antecedentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Antecedentes == null)
            {
                return NotFound();
            }

            var antecedente = await _context.Antecedentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (antecedente == null)
            {
                return NotFound();
            }

            return View(antecedente);
        }

        // GET: Antecedentes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Antecedentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DescripcionAntecedente")] Antecedente antecedente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(antecedente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(antecedente);
        }

        // GET: Antecedentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Antecedentes == null)
            {
                return NotFound();
            }

            var antecedente = await _context.Antecedentes.FindAsync(id);
            if (antecedente == null)
            {
                return NotFound();
            }
            return View(antecedente);
        }

        // POST: Antecedentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DescripcionAntecedente")] Antecedente antecedente)
        {
            if (id != antecedente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(antecedente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AntecedenteExists(antecedente.Id))
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
            return View(antecedente);
        }

        // GET: Antecedentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Antecedentes == null)
            {
                return NotFound();
            }

            var antecedente = await _context.Antecedentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (antecedente == null)
            {
                return NotFound();
            }

            return View(antecedente);
        }

        // POST: Antecedentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Antecedentes == null)
            {
                return Problem("Entity set 'CitasMedicasContext.Antecedentes'  is null.");
            }
            var antecedente = await _context.Antecedentes.FindAsync(id);
            if (antecedente != null)
            {
                _context.Antecedentes.Remove(antecedente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AntecedenteExists(int id)
        {
          return (_context.Antecedentes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

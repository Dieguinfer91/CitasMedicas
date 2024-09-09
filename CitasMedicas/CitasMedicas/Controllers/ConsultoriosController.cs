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
    public class ConsultoriosController : Controller
    {
        private readonly CitasMedicasContext _context;

        public ConsultoriosController(CitasMedicasContext context)
        {
            _context = context;
        }

        // GET: Consultorios
        public async Task<IActionResult> Index()
        {
            var consultorios = await _context.Consultorios
            .Include(c => c.Medico)
            .ToListAsync();


            return consultorios != null ? 
                View(await _context.Consultorios.ToListAsync()) :
                Problem("Entity set 'CitasMedicasContext.Consultorios'  is null.");
        }

        // GET: Consultorios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Consultorios == null)
            {
                return NotFound();
            }

            var consultorio = await _context.Consultorios
                .Include(c=>c.Medico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultorio == null)
            {
                return NotFound();
            }

            return View(consultorio);
        }

        // GET: Consultorios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consultorios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreConsultorio,DescripcionConsultorio")] Consultorio consultorio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consultorio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consultorio);
        }

        // GET: Consultorios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Consultorios == null)
            {
                return NotFound();
            }

            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio == null)
            {
                return NotFound();
            }
            return View(consultorio);
        }

        // POST: Consultorios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreConsultorio,DescripcionConsultorio")] Consultorio consultorio)
        {
            if (id != consultorio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultorio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultorioExists(consultorio.Id))
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
            return View(consultorio);
        }

        // GET: Consultorios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Consultorios == null)
            {
                return NotFound();
            }

            var consultorio = await _context.Consultorios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultorio == null)
            {
                return NotFound();
            }

            return View(consultorio);
        }

        // POST: Consultorios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Consultorios == null)
            {
                return Problem("Entity set 'CitasMedicasContext.Consultorios'  is null.");
            }
            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio != null)
            {
                _context.Consultorios.Remove(consultorio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultorioExists(int id)
        {
          return (_context.Consultorios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

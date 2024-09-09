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
    public class CitasController : Controller
    {
        private readonly CitasMedicasContext _context;

        public CitasController(CitasMedicasContext context)
        {
            _context = context;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            string? userName = HttpContext.Session.GetString("UserName");
            string? TipoUsuario = HttpContext.Session.GetString("TipoUsuario");
            if (userName != null &&( TipoUsuario=="Medico"))
            {
                var citasMedicasContext = _context.Citas.Include(c => c.Medico).Include(c => c.Paciente).Where(c => c.Medico.Usuario.UserName == userName && c.EstadoCita == 1);
                return View("Index", await citasMedicasContext.ToListAsync());
            }
            else if (userName != null && (TipoUsuario == "Paciente"))
            {
                var citasMedicasContext = _context.Citas.Include(c => c.Medico).Include(c => c.Paciente).Where(c => c.Paciente.Usuario.UserName == userName);
                return View("Index", await citasMedicasContext.ToListAsync());
            }
            else
            {
                var citasMedicasContext = _context.Citas.Include(c => c.Medico).Include(c => c.Paciente);
                return View(await citasMedicasContext.ToListAsync());
            }
            
        }
        // GET: Citas
        public async Task<IActionResult> CitasMedico()
        {
            string? userName = HttpContext.Session.GetString("UserName");
            if(userName != null)
            {
                var citasMedicasContext = _context.Citas.Include(c => c.Medico).Include(c => c.Paciente).Where(c => c.Medico.Usuario.UserName == userName);
                return View("Index",await citasMedicasContext.ToListAsync());
            }
            else
            {
                return NotFound();
            }
            
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }


        // GET: Citas/Create
        [HttpPost]
        public IActionResult ReturnCreate([Bind("PacienteId,PacienteNombre,EspecialidadId")] CreateCitaViewModel createCitaViewModel)
        {
            ViewData["ConsultorioId"] = new SelectList(
                _context.Consultorios, 
                "Id",              
                "NombreConsultorio"
            );
            ViewData["MedicoId"] = new SelectList(
                _context.Medicos.Where(m=>m.Especialidades.FirstOrDefault(e=>e.Id== createCitaViewModel.EspecialidadId) !=null),
                "Id",
                "NombreCompleto"
            );
            ViewData["PacienteId"] = new SelectList(
                _context.Pacientes,
                "Id",
                "NombrePaciente"
            );
            ViewBag.NombrePaciente = createCitaViewModel.PacienteNombre;
            ViewBag.IdPaciente = createCitaViewModel.PacienteId;
            return View("Create");
        }

        // POST: Citas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FechaCita,HoraCita,MedicoId,DiagnosticoId,ConsultorioId,PacienteId,EstadoCita")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                var citaEncontrada = await _context.Citas
                .Where(c => c.HoraCita == cita.HoraCita && c.FechaCita == cita.FechaCita
                &&c.MedicoId==cita.MedicoId)
                .FirstOrDefaultAsync();
                var citaEncontrada2 = await _context.Citas
                .Where(c => c.HoraCita == cita.HoraCita && c.FechaCita == cita.FechaCita
                && c.PacienteId == cita.PacienteId)

                .FirstOrDefaultAsync();
                var citaEncontrada3 = await _context.Citas
                .Where(c => c.HoraCita == cita.HoraCita && c.FechaCita == cita.FechaCita)

                .FirstOrDefaultAsync();


                if (citaEncontrada != null || citaEncontrada2 != null || citaEncontrada3 != null)
                {
                    
                    if (citaEncontrada != null) ModelState.AddModelError("MedicoId", "El medico esta ocupado");
                    if (citaEncontrada2 != null) ModelState.AddModelError("PacienteId", "Paciente ya tiene una cita en ese horario");
                    if (citaEncontrada3 != null) ModelState.AddModelError("ConsultorioId", "El consultorio está ocupado");

            //        ViewData["ConsultorioId"] = new SelectList(
            //    _context.Consultorios,
            //    "Id",
            //    "NombreConsultorio",
            //    cita.ConsultorioId
            //);
                    ViewData["MedicoId"] = new SelectList(
                        _context.Medicos,
                        "Id",
                        "NombreCompleto",
                        cita.MedicoId
                    );
                    ViewData["PacienteId"] = new SelectList(
                        _context.Pacientes,
                        "Id",
                        "NombrePaciente",
                        cita.PacienteId
                    );
                    return View(cita);
                }


                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsultorioId"] = new SelectList(
                _context.Consultorios,
                "Id",
                "NombreConsultorio"
            );
            ViewData["MedicoId"] = new SelectList(
                _context.Medicos,
                "Id",
                "NombreCompleto"
            );
            ViewData["PacienteId"] = new SelectList(
                _context.Pacientes,
                "Id",
                "NombrePaciente"
            );
            return View(cita);
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            //var cita = await _context.Citas.FindAsync(id);
            var cita = await _context.Citas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .Include(c => c.Receta).ThenInclude(mr => mr.Medicamento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }
            
            ViewData["MedicoId"] = new SelectList(
                _context.Medicos,
                "Id",
                "NombreCompleto",
                cita.MedicoId
            );
            ViewData["PacienteId"] = new SelectList(
                _context.Pacientes,
                "Id",
                "NombrePaciente",
                cita.PacienteId
            );
            HistoriaClinica historiaClinica = await _context.HistoriasClinicas
                .Where(h => h.PacienteId == cita.PacienteId)
                .FirstOrDefaultAsync();

            ViewBag.HistoriaClinica = historiaClinica;
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaCita,HoraCita,MedicoId,DiagnosticoId,ConsultorioId,PacienteId,EstadoCita,Diagnostico")] Cita cita)
        {

            if (HttpContext.Session.GetString("TipoUsuario") == "Paciente")
            {
                var citaPaciente= await _context.Citas.FindAsync(id);
                if(citaPaciente!=null)
                {
                    citaPaciente.EstadoCita = 3;
                    _context.Update(citaPaciente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }

            if (id != cita.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.Id))
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
            //ViewData["ConsultorioId"] = new SelectList(
            //    _context.Consultorios,
            //    "Id",
            //    "NombreConsultorio",
            //    cita.ConsultorioId
            //);
            
            
            ViewData["MedicoId"] = new SelectList(
                _context.Medicos,
                "Id",
                "NombreCompleto",
                cita.MedicoId
            );
            
            ViewData["PacienteId"] = new SelectList(
                _context.Pacientes,
                "Id",
                "NombrePaciente",
                cita.PacienteId
            );
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Citas == null)
            {
                return Problem("Entity set 'CitasMedicasContext.Citas'  is null.");
            }
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult SeleccionarEspecialidad([Bind("PacienteId,PacienteNombre")] CreateCitaViewModel createCitaViewModel)
        {
               
            ViewData["EspecialidadId"] = new SelectList(
                    _context.Especialidades,
                    "Id",
                    "DescripcionEspecialidad"
                );
            ViewBag.NombrePaciente = createCitaViewModel.PacienteNombre;
            ViewBag.IdPaciente = createCitaViewModel.PacienteId;

            return View("SeleccionarEspecialidad");
        }

        private bool CitaExists(int id)
        {
          return (_context.Citas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

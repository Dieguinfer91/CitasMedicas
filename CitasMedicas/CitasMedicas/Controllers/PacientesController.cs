using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Data;
using CitasMedicas.Models;
using Microsoft.AspNetCore.Identity;
using CitasMedicas.Middleware;

namespace CitasMedicas.Controllers
{
    public class PacientesController : Controller
    {
        private readonly CitasMedicasContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly EmailService _emailService;

        public PacientesController(CitasMedicasContext context, UserManager<Usuario> userManager, EmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            var citasMedicasContext = _context.Pacientes.Include(p => p.Usuario);
            return View(await citasMedicasContext.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CedulaPaciente,NombrePaciente,FechaNacimientoPaciente,CorreoPaciente,TelefonoPaciente,DiagnosticoPaciente,EdadPaciente")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = paciente.CorreoPaciente,
                    Email = paciente.CorreoPaciente,
                    TipoUsuario = "Paciente",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    PhoneNumber = ""
                };


                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/templates/emailTemplate.html");
                var emailBody = await System.IO.File.ReadAllTextAsync(templatePath);

                string randomPassword = _emailService.GenerateRandomPassword(10);

                var existingUser = await _userManager.FindByEmailAsync(paciente.CorreoPaciente);

                if(existingUser!=null)
                {
                   ModelState.AddModelError("CorreoPaciente", "El correo ya se encuentra registrado");
                    return View(paciente);
                }

                //var result = await _userManager.CreateAsync(user, randomPassword);

                var result = await _userManager.CreateAsync(user, "Default123");

                emailBody = emailBody.Replace("{{CodigoAcceso}}", randomPassword);


                
                _emailService.SendEmailAsync(paciente.CorreoPaciente, "Credenciales de acceso", emailBody);

                                
                if (result.Succeeded)
                {
                    var usuario = await _userManager.FindByEmailAsync(paciente.CorreoPaciente);
                    paciente.UsuarioId = usuario.Id;
                    _context.Add(paciente);
                    await _context.SaveChangesAsync();
                    HistoriaClinica historiaClinica = new HistoriaClinica
                    {
                        PacienteId = paciente.Id
                    };

                    _context.Add(historiaClinica);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", paciente.UsuarioId);
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", paciente.UsuarioId);
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CedulaPaciente,NombrePaciente,FechaNacimientoPaciente,CorreoPaciente,TelefonoPaciente,UsuarioId,DiagnosticoPaciente,EdadPaciente,PacienteId")] Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(paciente);
                    
                    
                    Usuario usuarioPaciente = await _userManager.FindByIdAsync(paciente.UsuarioId);
                    usuarioPaciente.Email = paciente.CorreoPaciente;
                    usuarioPaciente.UserName = paciente.CorreoPaciente;
                    await _userManager.UpdateAsync(usuarioPaciente);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", paciente.UsuarioId);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pacientes == null)
            {
                return Problem("Entity set 'CitasMedicasContext.Pacientes'  is null.");
            }
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
          return (_context.Pacientes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

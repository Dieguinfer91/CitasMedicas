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
    public class MedicosController : Controller
    {
        private readonly CitasMedicasContext _context;//inyeccion de dependendecias, pasan a ser globales para ir inicializando, permite que la clase controlador reciba la clase de contexto y que este contexto sea un objeto global de toda la aplicacion
        private readonly UserManager<Usuario> _userManager;
        private readonly EmailService _emailService;
        public MedicosController(CitasMedicasContext context, UserManager<Usuario> userManager, EmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            var citasMedicasContext = _context.Medicos.Include(m => m.Especialidades).Include(m => m.Usuario).Include(m=>m.Consultorio);
            return View(await citasMedicasContext.ToListAsync());
        }

        // GET: Medicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .Include(m => m.Especialidades)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // GET: Medicos/Create
        public IActionResult Create()
        {
            //ViewData["EspecialidadId"] = new SelectList(
            //        _context.Especialidades, // Fuente de datos
            //        "Id",                    // Valor del atributo
            //        "DescripcionEspecialidad"
            //    );

            ViewData["ConsultorioId"] = new SelectList(
                    _context.Consultorios.Include(c => c.Medico).Where(c => c.Medico == null),
                    "Id",                    // Valor del atributo
                    "NombreConsultorio"
                );


            return View();
        }

        // POST: Medicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CedulaMedico,NombreMedico,ApellidoMedico,CorreoMedico,TelefonoMedico,EspecialidadId,FechaNacimientoMedico,ConsultorioId")] Medico medico)
        {

            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = medico.CorreoMedico,
                    Email = medico.CorreoMedico,
                    TipoUsuario = "Medico",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    PhoneNumber = ""
                };

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/templates/emailTemplate.html");
                var emailBody = await System.IO.File.ReadAllTextAsync(templatePath);

                string randomPassword = _emailService.GenerateRandomPassword(10);

                var existingUser = await _userManager.FindByEmailAsync(medico.CorreoMedico);

                if (existingUser != null)
                {
                    ModelState.AddModelError("CorreoMedico", "El correo ya se encuentra registrado");
                    ViewData["ConsultorioId"] = new SelectList(
                    _context.Consultorios.Include(c => c.Medico).Where(c => c.Medico == null),
                    "Id",                  
                    "NombreConsultorio"
                );
                    return View(medico);
                }

                //var result = await _userManager.CreateAsync(user, randomPassword);

                var result = await _userManager.CreateAsync(user, "Default123");

                emailBody = emailBody.Replace("{{CodigoAcceso}}", randomPassword);


                _emailService.SendEmailAsync(medico.CorreoMedico, "Credenciales de acceso", emailBody);


                if (result.Succeeded)
                {
                    var usuario = await _userManager.FindByEmailAsync(medico.CorreoMedico);
                    medico.UsuarioId = usuario.Id;
                    _context.Add(medico);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }


            }
            ViewData["ConsultorioId"] = new SelectList(
                   _context.Consultorios.Include(c => c.Medico).Where(c => c.Medico == null),
                   "Id",
                   "NombreConsultorio"
               );
            return View(medico);
        }

        // GET: Medicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            ViewData["ConsultorioId"] = new SelectList(
                    _context.Consultorios.Include(c=>c.Medico).Where(c=>c.Medico==null || c.Medico.Id==medico.Id), 
                    "Id",                  
                    "NombreConsultorio",
                    medico.ConsultorioId
                );

            return View(medico);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEspecialidad(MedicoEspecialidadViewModel model)
        {
            ModelState.Remove("Especialidad");
            ModelState.Remove("Medico");

            if (ModelState.IsValid)
            {
                var medico = await _context.Medicos.FindAsync(model.MedicoId);
                var especialidad = await _context.Especialidades.FindAsync(model.EspecialidadId);
                if (medico != null && especialidad != null)
                {
                    medico.Especialidades.Add(especialidad);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Medicos", new { id = model.MedicoId });


                }
            }
            return View("AgregarEspecialidad", model);
        }

        public async Task<IActionResult> AgregarEspecialidad(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }

            ViewData["EspecialidadId"] = new SelectList(
                    _context.Especialidades.Include(m=>m.Medicos).Where(e=>!e.Medicos.Contains(medico)),
                    "Id",                   
                    "DescripcionEspecialidad"
                );
            
            return View(new MedicoEspecialidadViewModel{ Medico= medico });
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CedulaMedico,NombreMedico,ApellidoMedico,CorreoMedico,TelefonoMedico,EspecialidadId,FechaNacimientoMedico,UsuarioId,ConsultorioId")] Medico medico)
        {
            if (id != medico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medico);
                    Usuario usuarioPaciente = await _userManager.FindByIdAsync(medico.UsuarioId);
                    usuarioPaciente.Email = medico.CorreoMedico;
                    usuarioPaciente.UserName = medico.CorreoMedico;
                    await _userManager.UpdateAsync(usuarioPaciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.Id))
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

            return View(medico);
        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .Include(m => m.Especialidades)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medicos == null)
            {
                return Problem("Entity set 'CitasMedicasContext.Medicos'  is null.");
            }
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                _context.Medicos.Remove(medico);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
          return (_context.Medicos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

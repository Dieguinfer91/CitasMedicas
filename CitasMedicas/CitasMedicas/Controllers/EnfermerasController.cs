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
    public class EnfermerasController : Controller
    {
        private readonly CitasMedicasContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly EmailService _emailService;

        public EnfermerasController(CitasMedicasContext context, UserManager<Usuario> userManager, EmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: Enfermeras
        public async Task<IActionResult> Index()
        {
            var citasMedicasContext = _context.Enfermera.Include(e => e.Usuario);
            return View(await citasMedicasContext.ToListAsync());
        }

        // GET: Enfermeras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Enfermera == null)
            {
                return NotFound();
            }

            var enfermera = await _context.Enfermera
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enfermera == null)
            {
                return NotFound();
            }

            return View(enfermera);
        }

        // GET: Enfermeras/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Enfermeras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CedulaEnfermera,NombreEnfermera,ApellidoEnfermera,CorreoEnfermera,TelefonoEnfermera,FechaNacimientoEnfermera")] Enfermera enfermera)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = enfermera.CorreoEnfermera,
                    Email = enfermera.CorreoEnfermera,
                    TipoUsuario = "Enfermera",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    PhoneNumber = ""
                };


                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/templates/emailTemplate.html");
                var emailBody = await System.IO.File.ReadAllTextAsync(templatePath);

                string randomPassword = _emailService.GenerateRandomPassword(10);

                var existingUser = await _userManager.FindByEmailAsync(enfermera.CorreoEnfermera);

                if (existingUser != null)
                {
                    ModelState.AddModelError("CorreoEnfermera", "El correo ya se encuentra registrado");
                    return View(enfermera);
                }

                //var result = await _userManager.CreateAsync(user, randomPassword);

                var result = await _userManager.CreateAsync(user, "Default123");

                emailBody = emailBody.Replace("{{CodigoAcceso}}", randomPassword);



                _emailService.SendEmailAsync(enfermera.CorreoEnfermera, "Credenciales de acceso", emailBody);


                if (result.Succeeded)
                {
                    var usuario = await _userManager.FindByEmailAsync(enfermera.CorreoEnfermera);
                    enfermera.UsuarioId = usuario.Id;
                    _context.Add(enfermera);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }


            }
            ViewData["EnfermeraId"] = new SelectList(_context.Usuarios, "Id", "Id", enfermera.UsuarioId);
            return View(enfermera);
        }

        // GET: Enfermeras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Enfermera == null)
            {
                return NotFound();
            }

            var enfermera = await _context.Enfermera.FindAsync(id);
            if (enfermera == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", enfermera.UsuarioId);
            return View(enfermera);
        }


        public async Task<IActionResult> Edit(int id, [Bind("Id,CedulaEnfermera,NombreEnfermera,FechaNacimientoEnfermera,CorreoEnfermera,TelefonoEnfermera,UsuarioId,DiagnosticoEnfermera,EnfermeraId")] Enfermera enfermera)
        {
            if (id != enfermera.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(enfermera);


                    Usuario usuarioEnfermera = await _userManager.FindByIdAsync(enfermera.UsuarioId);
                    usuarioEnfermera.Email = enfermera.CorreoEnfermera;
                    usuarioEnfermera.UserName = enfermera.CorreoEnfermera;
                    await _userManager.UpdateAsync(usuarioEnfermera);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnfermeraExists(enfermera.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", enfermera.UsuarioId);
            return View(enfermera);
        }

        // POST: Enfermeras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,CedulaEnfermera,NombreEnfermera,ApellidoEnfermera,CorreoEnfermera,TelefonoEnfermera,FechaNacimientoEnfermera,UsuarioId")] Enfermera enfermera)
        //{
        //    if (id != enfermera.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(enfermera);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EnfermeraExists(enfermera.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", enfermera.UsuarioId);
        //    return View(enfermera);
        //}

        // GET: Enfermeras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Enfermera == null)
            {
                return NotFound();
            }

            var enfermera = await _context.Enfermera
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enfermera == null)
            {
                return NotFound();
            }

            return View(enfermera);
        }

        // POST: Enfermeras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Enfermera == null)
            {
                return Problem("Entity set 'CitasMedicasContext.Enfermera'  is null.");
            }
            var enfermera = await _context.Enfermera.FindAsync(id);
            if (enfermera != null)
            {
                _context.Enfermera.Remove(enfermera);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnfermeraExists(int id)
        {
          return (_context.Enfermera?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

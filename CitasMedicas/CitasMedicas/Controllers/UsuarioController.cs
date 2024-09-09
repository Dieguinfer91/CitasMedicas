using CitasMedicas.Data;
using CitasMedicas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitasMedicas.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly CitasMedicasContext _context;
        public UsuarioController(CitasMedicasContext context, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var usuario = await _userManager.FindByEmailAsync(model.Email);

                    if (usuario != null)
                    {
                        if(usuario.PhoneNumberConfirmed)
                        {
                            HttpContext.Session.SetString("UserName", usuario.UserName);
                            HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario);
                            HttpContext.Session.SetInt32("EstadoLogin", 1);
                            if(usuario.TipoUsuario=="Paciente")
                            {
                                
                                var paciente=_context.Pacientes.Include(p=>p.Usuario).Where(p=>p.Usuario.UserName== usuario.UserName).FirstOrDefault();
                                if(paciente!=null)
                                {
                                    HttpContext.Session.SetInt32("PacienteId", paciente.Id);
                                    HttpContext.Session.SetString("PacienteNombre", paciente.NombrePaciente);
                                }
                            }
                        }
                        else
                        {
                            HttpContext.Session.SetString("UserName", usuario.UserName);
                            HttpContext.Session.SetInt32("EstadoLogin", 0);
                            return RedirectToAction("ChangePassword", "Usuario");
                        }
                    }
                    return RedirectToAction("Dashboard", "Home");
                }

                ModelState.AddModelError(string.Empty, "Credenciales incorrectas");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? userName=HttpContext.Session.GetString("UserName");
                if(userName!=null)
                {
                    var usuario = await _userManager.FindByEmailAsync(userName);
                    if (usuario == null)
                    {
                        return RedirectToAction("Login", "Usuario");
                    }
                    else
                    {
                        var result = await _userManager.ChangePasswordAsync(usuario, model.CurrentPassword, model.NewPassword);
                        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(usuario, "");

                        var resultPhone = await _userManager.ChangePhoneNumberAsync(usuario, usuario.PhoneNumber, token);
                        if (result.Succeeded && resultPhone.Succeeded)
                        {
                            await _signInManager.SignOutAsync();
                            HttpContext.Session.Clear();
                            return RedirectToAction("Login", "Usuario");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario { UserName = model.Email, Email = model.Email,TipoUsuario="Admin",
                PhoneNumberConfirmed=true,TwoFactorEnabled=false,LockoutEnabled=false,AccessFailedCount=0,
                PhoneNumber="0987171309"};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}




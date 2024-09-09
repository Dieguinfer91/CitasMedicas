using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

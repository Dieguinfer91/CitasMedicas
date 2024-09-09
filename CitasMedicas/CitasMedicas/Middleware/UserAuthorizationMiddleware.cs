namespace CitasMedicas.Middleware
{
    public class UserAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public UserAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Evitar bucle infinito al redirigir a páginas de login y acceso denegado
            var path = context.Request.Path.ToString().ToLower();
            if (path.StartsWith("/usuario/login") ||path.Equals("/") || path.StartsWith("/accessdenied")
                || path.StartsWith("/usuario/changepassword")
                || path.StartsWith("/usuario/register")
                || path.StartsWith("/email/SendEmail")
                || path.StartsWith("/usuario/logout"))
            {
                await _next(context);
                return;
            }

            var user = context.User;
            if (user.Identity.IsAuthenticated)
            {
                var tipoUsuario = context.Session.GetString("TipoUsuario");

                if (string.IsNullOrEmpty(tipoUsuario))
                {
                    context.Response.Redirect("/Usuario/Login");
                    return;
                }

                if (!HasAccessToRoute(context, tipoUsuario))
                {
                    context.Response.Redirect("/AccessDenied");
                    return;
                }
            }
            else
            {
                if(!path.Equals("/"))
                {
                    context.Response.Redirect("/Usuario/Login");
                    return;
                }
                
            }

            await _next(context);
        }

        private bool HasAccessToRoute(HttpContext context, string tipoUsuario)
        {
            // Aquí puedes implementar la lógica para verificar las rutas permitidas para cada tipo de usuario
            // Por ejemplo:
            var path = context.Request.Path.ToString().ToLower();

            if(path.Equals("/"))
            {
                return true;
            }

            if (tipoUsuario == "Admin")
            {
                return true; // Admin tiene acceso a todas las rutas
            }

            if (tipoUsuario == "Medico" && (path.StartsWith("/medico")
                || path.StartsWith("/home/dashboard")
                || path.StartsWith("/citas"))
                || path.StartsWith("/historiasclinicas")
                || path.StartsWith("/medicamentosreceta")
                )

            {
                return true;
            }

            if (tipoUsuario == "Trabajador" && path.StartsWith("/trabajador"))
            {
                return true;
            }

            if (tipoUsuario == "Paciente" && (path.StartsWith("/paciente") 
                || path.StartsWith("/home/dashboard")
                || path.StartsWith("/citas")
                || path.StartsWith("/historiasclinicas")
                ))
            {
                return true;
            }

            return false; // Denegar acceso por defecto
        }
    }

}

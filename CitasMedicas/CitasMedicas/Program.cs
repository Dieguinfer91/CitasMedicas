using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CitasMedicas.Data;
using Microsoft.AspNetCore.Identity;
using CitasMedicas.Models;
using CitasMedicas.Middleware;
namespace CitasMedicas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
          //añado el dbcontext para que comporte como un servicio
            builder.Services.AddDbContext<CitasMedicasContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CitasMedicasContext") ?? throw new InvalidOperationException("Connection string 'CitasMedicasContext' not found.")));

            //instancia la administrar CRUD y permisos a módulas
            
            builder.Services.AddDefaultIdentity<Usuario>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
            })
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<CitasMedicasContext>();


            //manejo de los cookies
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;//solo trabaje a travez de este protocolo
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);//exipración de la cookie
                options.LoginPath = "/Usuario/Login";//cuando la cookie expira me dirige a la página de inicio de sessión
                options.AccessDeniedPath = "/AccessDenied";//a que página me redirige cuando se deniega el acceso
                options.SlidingExpiration = true;// si reinicio una cookie válida y reinicia el contadoer de tiempo
            });


            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60); // Set session timeout
                options.Cookie.HttpOnly = true;//protocolo
                options.Cookie.IsEssential = true;//si no inicias session no funciona
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews();//instancia loscontroladores desarrollados y las vistas
            builder.Services.AddTransient<EmailService>();//instancia servicio de  email

            var app = builder.Build();

            // Configure the HTTP request pipeline. Manejo de expciones
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();//protocolo seguro
            app.UseStaticFiles();//pueda utilizar imagenes y ahorra memoria si no la usas

            app.UseRouting();//hacer urls dinámicas

            app.UseAuthentication();//para acceso via login
            app.UseAuthorization();//te proge la aplicación

            app.UseSession();//establece la session
            app.UseMiddleware<UserAuthorizationMiddleware>();//es como usar una librería en este caso la autorización de usuarios

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");//coo contruyo las rutas-plantilla (como se construye la ruta)
            app.MapControllerRoute(//establece cómo redirige a la ruta de denegación de acceso
                name: "accessdenied",
                pattern: "AccessDenied",
                defaults: new { controller = "Error", action = "AccessDenied" });

            //CreateRoles(app.Services).Wait();

            app.Run();//inicialiaza la app (análogo a form.show)
        }

        //private static async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    string[] roleNames = { "Administrador", "Medico", "Trabajador", "Paciente" };

        //    foreach (var roleName in roleNames)
        //    {
        //        var roleExist = await roleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            await roleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }
        //}
    }
}
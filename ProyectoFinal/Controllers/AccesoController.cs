using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Data;
using ProyectoFinal.Models;
using ProyectoFinal.ViewModels;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ProyectoFinal.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public AccesoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioVM modelo)
        {
            if (modelo.contraseña != modelo.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
            }

            Usuario usuario = new Usuario()
            {
                Nombre_usuario = modelo.Nombre_usuario,
                email = modelo.email,
                contraseña = modelo.contraseña,
            };

            await _appDBContext.Usuarios.AddAsync(usuario);
            await _appDBContext.SaveChangesAsync();

            if (usuario.Id_usuario != 0) return RedirectToAction("Login", "Acceso");

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM modelo)
        {
            Usuario? usuario_encontrado = await _appDBContext.Usuarios.Where(u => u.email == modelo.email && u.contraseña == modelo.contraseña).FirstOrDefaultAsync();

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron usuarios";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre_usuario)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("DashboardUsuario", "Usuario");
        }

        public IActionResult LoginPaciente()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoginPaciente(LoginVM modelo)
        {
            Paciente usuario_encontrado = await _appDBContext.Pacientes.Where(u => u.email == modelo.email && u.contraseña == modelo.contraseña).FirstOrDefaultAsync();

            if (usuario_encontrado != null && usuario_encontrado.estado)
            {
                List<Claim> claims = new List<Claim>()
            {
            new Claim(ClaimTypes.Name, usuario_encontrado.email)// + " " + usuario_encontrado.ApellidoPaterno + " " + usuario_encontrado.ApellidoMaterno)
            };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                return RedirectToAction("Dashboard", "Paciente");
            }
            else
            {
                ViewData["Mensaje"] = "No se encontraro usuario o el usuario está inactivo.";
                return View();
            }
        }

    }
}

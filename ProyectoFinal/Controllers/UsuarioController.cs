using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public UsuarioController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> ListaUsuario()
        {
            List<Usuario> lista = await _appDBContext.Usuarios.ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> DashboardUsuario()
        {
            return View();
        }

    }
}

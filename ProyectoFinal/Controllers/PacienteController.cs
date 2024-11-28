using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using ProyectoFinal.Data;
using Microsoft.AspNetCore.Identity;

namespace ProyectoFinal.Controllers
{
    public class PacienteController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public PacienteController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Paciente> lista = await _appDBContext.Pacientes.ToListAsync();
            return View(lista);
        }



        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(Paciente paciente)
        {
            await _appDBContext.Pacientes.AddAsync(paciente);
            await _appDBContext.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Paciente paciente = await _appDBContext.Pacientes.FirstAsync(e => e.idPaciente == id);
            return View(paciente);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Paciente paciente)
        {
            _appDBContext.Pacientes.Update(paciente);
            await _appDBContext.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> CambiarClave(string id)
        {
            Paciente paciente = await _appDBContext.Pacientes.FirstAsync(e => e.email == id);
            return View(paciente);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarClave(Paciente paciente)
        {
            var pacienteActual = await _appDBContext.Pacientes.FindAsync(paciente.idPaciente);

            pacienteActual.contraseña = paciente.contraseña;
			await _appDBContext.SaveChangesAsync();

			return RedirectToAction(nameof(Dashboard));
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using ProyectoFinal.Data;
using Microsoft.AspNetCore.Identity;

namespace ProyectoFinal.Controllers
{
    public class AnalisisController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public AnalisisController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> ListaAnalisis()
        {
            List<Analisis> lista = await _appDBContext.AnalisisC.ToListAsync();
            return View(lista);
        }
        [HttpGet]
        public async Task<IActionResult> NuevoAnalisis()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NuevoAnalisis(Analisis analisis)
        {
            await _appDBContext.AnalisisC.AddAsync(analisis);
            await _appDBContext.SaveChangesAsync();
            return RedirectToAction(nameof(ListaAnalisis));
        }

        [HttpGet]
        public async Task<IActionResult> EditarAnalisis(int id)
        {
            Analisis analisis = await _appDBContext.AnalisisC.FirstAsync(e => e.idAnalisis == id);
            return View(analisis);
        }

        [HttpPost]
        public async Task<IActionResult> EditarAnalisis(Analisis analisis)
        {
            _appDBContext.AnalisisC.Update(analisis);
            await _appDBContext.SaveChangesAsync();
            return RedirectToAction(nameof(ListaAnalisis));
        }
    }
}

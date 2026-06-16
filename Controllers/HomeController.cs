using cromosmundial_proyecto_final.Data;
using cromosmundial_proyecto_final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace cromosmundial_proyecto_final.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["TotalJugadores"] = await _context.Jugadores.CountAsync();
            ViewData["TotalSelecciones"] = await _context.Selecciones.CountAsync();
            ViewData["TotalCromos"] = await _context.Cromos.CountAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
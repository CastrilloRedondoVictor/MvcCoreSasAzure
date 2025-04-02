using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSasAzure.Models;
using MvcCoreSasAzure.Services;

namespace MvcCoreSasAzure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ServiceAzureAlumnos serviceAzureAlumnos;

        public HomeController(ILogger<HomeController> logger, ServiceAzureAlumnos serviceAzureAlumnos)
        {
            _logger = logger;
            this.serviceAzureAlumnos = serviceAzureAlumnos;
        }

        public async Task<IActionResult> Index()
        {
            List<Alumno> alumnos = await this.serviceAzureAlumnos.GetAlumnosAsync("EN PROCESO");
            return View(alumnos);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Alumno alumno)
        {
            await this.serviceAzureAlumnos.AddAlumnoAsync(alumno);
            return RedirectToAction("Index");
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

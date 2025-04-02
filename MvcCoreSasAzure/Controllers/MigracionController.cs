using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSasAzure.Helpers;
using MvcCoreSasAzure.Models;

namespace MvcCoreSasAzure.Controllers
{
    public class MigracionController : Controller
    {
        private HelperXml helperXml;

        public MigracionController(HelperXml helper)
        {
            this.helperXml = helper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string accion)
        {
            string azureKeys = "DefaultEndpointsProtocol=https;AccountName=storagetajamarvcr;AccountKey=HUvi/0yah8KpfydLnSfM+MHF8PfpVdwGJUJcsPOY3RR0xLYgD/PD2o3GBNPlLJPNGhzm69Mfq0Cp+ASt4fpzqg==;EndpointSuffix=core.windows.net";
            TableServiceClient tableServiceClient = new TableServiceClient(azureKeys);
            TableClient tableClient = tableServiceClient.GetTableClient("alumnos");
            await tableClient.CreateIfNotExistsAsync();

            List<Alumno> alumnos = this.helperXml.GetAlumnos();
            foreach (Alumno alumno in alumnos)
            {
                await tableClient.AddEntityAsync(alumno);
            }

            ViewData["Migracion"] = "Migración realizada";
            return View();
        }
    }
}

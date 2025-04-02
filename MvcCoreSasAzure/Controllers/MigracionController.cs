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
            string azureKeys = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
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

using Azure.Data.Tables;
using MvcCoreSasAzure.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace MvcCoreSasAzure.Services
{
    public class ServiceAzureAlumnos
    {
        private TableClient tableClient;
        private string UrlApi;

        public ServiceAzureAlumnos(IConfiguration configuration)
        {
            UrlApi = configuration["ApiUrls:ApiAzureToken"];
        }

        private async Task<string> GetApiTokenAsync(string curso)
        {
            string request = "token/" + curso;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(UrlApi);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(data);
                string token = json.GetValue("token").ToString();
                return token;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Alumno>> GetAlumnosAsync(string curso)
        {
            string token = await GetApiTokenAsync(curso);

            Uri uriToken = new Uri(token);

            this.tableClient = new TableClient(uriToken);
            List<Alumno> alumnos = new List<Alumno>();

            await foreach (Alumno alumno in this.tableClient.QueryAsync<Alumno>())
            {
                alumnos.Add(alumno);
            }

            return alumnos;
        }

        public async Task AddAlumnoAsync(Alumno alumno)
        {
            string token = await GetApiTokenAsync(alumno.Curso);
            Uri uriToken = new Uri(token);
            this.tableClient = new TableClient(uriToken);
            await this.tableClient.AddEntityAsync<Alumno>(alumno);
        }
    }
}

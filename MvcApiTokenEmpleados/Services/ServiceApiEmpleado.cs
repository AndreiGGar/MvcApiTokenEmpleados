using MvcApiTokenEmpleados.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MvcApiTokenEmpleados.Services
{
    public class ServiceApiEmpleados
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiEmpleados;

        public ServiceApiEmpleados(IConfiguration configuration)
        {
            this.UrlApiEmpleados = configuration.GetValue<string>("ApiUrls:ApiOAuthEmpleados");
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetTokenAsync
            (string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Auth/Login";
                client.BaseAddress = new Uri(this.UrlApiEmpleados);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    UserName = username,
                    Password = password
                };
                string jsonModel = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(jsonModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(data);
                    string token = jsonObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiEmpleados);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiEmpleados);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Empleado>> GetEmpleadosAsync(string token)
        {
            string request = "api/Empleados";
            List<Empleado> empleados =
                await this.CallApiAsync<List<Empleado>>(request, token);
            return empleados;
        }

        public async Task<Empleado> FindEmpleadoAsync(int idempleado)
        {
            string request = "api/Empleados/" + idempleado;
            Empleado empleado =
                await this.CallApiAsync<Empleado>(request);
            return empleado;
        }

        public async Task<Empleado> GetPerfilEmpleadoAsync(string token)
        {
            string request = "api/Empleados/perfilempleado";
            Empleado empleado = await this.CallApiAsync<Empleado>(request, token);
            return empleado;
        }

        public async Task<List<Empleado>> GetCompisCurroAsync(string token)
        {
            string request = "api/Empleados/compiscurro";
            List<Empleado> compis = await this.CallApiAsync<List<Empleado>>(request, token);
            return compis;
        }
    }
}

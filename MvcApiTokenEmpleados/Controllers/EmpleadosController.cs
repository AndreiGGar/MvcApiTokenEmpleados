using Microsoft.AspNetCore.Mvc;
using MvcApiTokenEmpleados.Filters;
using MvcApiTokenEmpleados.Models;
using MvcApiTokenEmpleados.Services;

namespace MvcApiTokenEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private ServiceApiEmpleados service;

        public EmpleadosController(ServiceApiEmpleados service)
        {
            this.service = service;
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Index()
        {
            string token =
                HttpContext.Session.GetString("TOKEN");
            if (token == null)
            {
                ViewData["MENSAJE"] = "Debe realizar Login para visualizar datos";
                return View();
            }
            else
            {
                List<Empleado> empleados = await this.service.GetEmpleadosAsync(token);
                return View(empleados);
            }
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Details(int id)
        {
            Empleado empleado = await this.service.FindEmpleadoAsync(id);
            return View(empleado);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Perfil()
        {
            string token = HttpContext.Session.GetString("TOKEN");
            Empleado empleado = await this.service.GetPerfilEmpleadoAsync(token);
            return View(empleado);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Compis()
        {
            string token = HttpContext.Session.GetString("TOKEN");
            List<Empleado> compis = await this.service.GetCompisCurroAsync(token);
            return View(compis);
        }
    }
}

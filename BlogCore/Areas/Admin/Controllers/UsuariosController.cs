using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;

        }

        [HttpGet]
        public IActionResult Index()
        {
            //En esta línea, se obtiene la identidad del usuario actual que está navegando en el sitio web
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contenedorTrabajo.Usuario.GetAll(item => item.Id != usuarioActual.Value));
        }

        [HttpGet]
        public IActionResult Bloquear(string Id)
        {
            if(Id == null)
            {
                return NotFound();

            }
            _contenedorTrabajo.Usuario.BloquearUsuario(Id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Desbloquear(string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}

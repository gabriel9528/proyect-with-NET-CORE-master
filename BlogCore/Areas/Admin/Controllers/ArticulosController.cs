using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _webHostEnvironment = webHostEnvironment;
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            return View(articuloVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM articuloVM)
        {
            if(ModelState.IsValid) 
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if(articuloVM.Articulo.Id == 0)
                {
                    //Creamos un nuevo articulo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using(var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension),FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    articuloVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.Add(articuloVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
            }

            articuloVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(articuloVM);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            if(id != null) 
            {
                articuloVM.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }

            return View(articuloVM);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM articuloVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var archivoDesdeDb = _contenedorTrabajo.Articulo.Get(articuloVM.Articulo.Id);

                if (archivos.Count()>0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, archivoDesdeDb.UrlImagen.TrimStart('\\'));

                    if(System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }


                    //nuevamente subimos el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    articuloVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //cuando la imagen ya existe y se conserva
                    articuloVM.Articulo.UrlImagen = archivoDesdeDb.UrlImagen;
                }

                _contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(articuloVM);
        }


        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
        }


        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            var rutaPrincipal = _webHostEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

            if(System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            if(articuloDesdeDb == null)
            {
                return Json(new {success= false, message="Error al borrar el articulo"});
            }

            _contenedorTrabajo.Articulo.Remove(articuloDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Articulo borrado correctamente" });

        }

        #endregion
    }
}

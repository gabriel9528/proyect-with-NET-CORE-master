using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlidersController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment webHostEnvironment)
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if(ModelState.IsValid) 
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                //we create a new slider
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                var nombreArchivo = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStream);
                }

                slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;

                _contenedorTrabajo.Slider.Add(slider);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

                
            }

            return View();
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {

            if(id != null) 
            {
                var slider = _contenedorTrabajo.Slider.Get(id.GetValueOrDefault());
                return View(slider);
            }

            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                var rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var sliderDesdeDb = _contenedorTrabajo.Slider.Get(slider.Id);

                if (archivos.Count() > 0)
                {
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, sliderDesdeDb.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Create again a new file

                    using (var fileStream = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream);
                    }

                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;

                    _contenedorTrabajo.Slider.Update(slider);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));

                }

                else
                {
                    slider.UrlImagen = sliderDesdeDb.UrlImagen;
                }

                _contenedorTrabajo.Slider.Update(slider);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }

            return View();
        }


        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Slider.GetAll() });
        }


        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            var sliderDesdeDb = _contenedorTrabajo.Slider.Get(id);
            

            if(sliderDesdeDb == null)
            {
                return Json(new {success= false, message="Error al borrar el slider"});
            }

            _contenedorTrabajo.Slider.Remove(sliderDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Slider borrado correctamente" });

        }

        #endregion
    }
}

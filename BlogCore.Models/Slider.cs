using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Slider
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Enter a name for the slider")]
        [Display(Name ="Name of the slider")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name ="State")]
        public bool Estado { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name ="Image")]
        public string UrlImagen { get; set; }
    }
}

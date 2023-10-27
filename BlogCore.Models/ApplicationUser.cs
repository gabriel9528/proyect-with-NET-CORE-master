using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogCore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "the name is obligatory")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "the direction is obligatory")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "the city is obligatory")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "the country is obligatory")]
        public string Pais { get; set; }
    }
}

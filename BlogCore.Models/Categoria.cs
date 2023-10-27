﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        
        
        [Required]
        [Display(Name = "Nombre de la categoria")]
        public String Nombre { get; set; }
        
        
        [Display(Name ="Orden de la visualizacion")]
        public int? Orden { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bib2.Models
{

    public partial class Lecturas
    {
		[Display(Name = "Título")]
        [MaxLength(50)]
        public string titulo { get; set; }
		[Display(Name = "Autor")]
        [MaxLength(50)]
        public string autor { get; set; }
        public int CodAutor { get; set; } 
		[Display(Name = "Leído")]
        public Nullable<System.DateTime> fecha { get; set; } 
		[Display(Name = "Calif.")]		
        public Nullable<int> calificacion { get; set; }
        [Display(Name = "Comentarios")] 
        public string comentario { get; set; }
		[Display(Name = "EBook")]
        public bool Ebook { get; set; }

        [Display(Name = "Comenzado")] 
        public Nullable<System.DateTime> fecha_Inicio { get; set; }
    }
}
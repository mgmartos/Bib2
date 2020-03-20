using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bib2.Models
{
    public class Libro
    {
        [Display(Name = "Código")]
        public int IdLibro;
        [Required]
        [Display(Name = "Título")]
        [StringLength(100)]
        public string titulo { get; set; }
        [Required]
        [Display(Name = "Autor")]
        [StringLength(100)]
        public string autor { get; set; }
        public string editorial { get; set; }
        public string coleccion { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fecha { get; set; }
        public string tema { get; set; }
        [Range(0,10)]
        public int calificacion { get; set; }
        [Display(Name = "Pág.")]
        public int paginas { get; set; }
        [StringLength(1000)]
        [Display(Name = "Comentarios")]
        public string comentario { get; set; }
        public string prestamo { get; set; }
        public DateTime fecha_prestamo { get; set; }
        public int numobras { get; set; }
        public string origen { get; set; }
        public int CodAutor { get; set; }

    }
}
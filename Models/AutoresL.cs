

namespace Bib2.Models
{
    public partial class AutoresL : Bib2.Models.Autores
    {
        public int Cantidad { get; set; }
        public AutoresL(int id, string n, int c)
        {
            this.idAutor = id;
            this.NombreAutor = n;
            this.Cantidad = c;
        }
    }
}
using System.Collections.Generic;
using L02P02_2022LA605_2022GO650.Models; // Asegúrate de que este namespace sea correcto y contenga la clase Libro

namespace L02P02_2022LA605_2022GO650.ViewModels
{
    public class CarritoViewModel
    {
        public List<Libros> Libros { get; set; } = new();
        public decimal Total { get; set; }
        public int CantidadLibros { get; set; }
    }
}

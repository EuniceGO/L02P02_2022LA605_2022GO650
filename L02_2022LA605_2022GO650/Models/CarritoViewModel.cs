using L02P02_2022LA605_2022GO650.Models;

namespace L02P02_2022LA605_2022GO650.ViewModels
{
    public class CarritoViewModel
    {
        public List<Libros> Libros { get; set; } = new List<Libros>();
        public decimal Total { get; set; }
        public int CantidadLibros { get; set; }
        public int IdCliente {  get; set; } 
    }
}

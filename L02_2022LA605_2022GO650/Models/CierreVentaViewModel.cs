namespace L02P02_2022LA605_2022GO650.ViewModels
{
    public class CierreVentaViewModel
    {
        public ClienteViewModel Cliente { get; set; } = new ClienteViewModel();
        public List<CarritoItemViewModel> Carrito { get; set; } = new List<CarritoItemViewModel>();
        public decimal Total => Carrito.Sum(item => item.Precio * item.Cantidad);
    }

    public class ClienteViewModel
    {
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Direccion { get; set; } = "";
    }

    public class CarritoItemViewModel
    {
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}

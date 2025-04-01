using L02P02_2022LA605_2022GO650.Models;
using L02P02_2022LA605_2022GO650.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace L02P02_2022LA605_2022GO650.Controllers
{
    public class CarritoController : Controller
    {
        private static List<pedido_detalle> carrito = new(); // Simulación del carrito

        private readonly libreriaDbContext _context;

        public CarritoController(libreriaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var libros = _context.Libros.ToList();
            var total = carrito.Sum(c => _context.Libros.Find(c.id)?.Precio ?? 0);

            var viewModel = new CarritoViewModel
            {
                Libros = libros,
                Total = total,
                CantidadLibros = carrito.Count
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AgregarAlCarrito(int idLibro)
        {
            var pedido = _context.pedido_encabezado.FirstOrDefault(p => p.id == 1); // Simulación de un pedido en progreso

            if (pedido != null)
            {
                var nuevoDetalle = new pedido_detalle
                {
                    id_pedido = pedido.id,
                    id_libro = idLibro,
                    dateTime = DateTime.Now
                };

                _context.pedido_detalle.Add(nuevoDetalle);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult CompletarCompra()
        {
            var pedido = _context.pedido_encabezado.FirstOrDefault(p => p.id == 1);

            if (pedido != null)
            {
                pedido.total = _context.pedido_detalle
                    .Where(d => d.id == pedido.id)
                    .Sum(d => d.id_libro);

                pedido.cantidad_libros= _context.pedido_detalle.Count(d => d.id_pedido == pedido.id);

                _context.SaveChanges();
            }

            return RedirectToAction("ProcesoVenta", "Venta");
        }


       
    }
}

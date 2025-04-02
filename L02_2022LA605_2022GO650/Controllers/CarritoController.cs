using L02P02_2022LA605_2022GO650.Models;
using L02P02_2022LA605_2022GO650.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace L02P02_2022LA605_2022GO650.Controllers
{
    public class CarritoController : Controller
    {
        private readonly libreriaDbContext _context;

        public CarritoController(libreriaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var libros = _context.Libros.ToList();
            var carrito = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == 1); // Simulación de cliente fijo

            var viewModel = new CarritoViewModel
            {
                Libros = libros,
                CantidadLibros = carrito?.cantidad_libros ?? 0,
                Total = carrito?.total ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AgregarAlCarrito(int idLibro)
        {
            var libro = _context.Libros.FirstOrDefault(l => l.id == idLibro);
            if (libro != null)
            {
                var carrito = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == 1);

                if (carrito == null)
                {
                    carrito = new pedido_encabezado
                    {
                        id_cliente = 1,
                        cantidad_libros = 1,
                        total = libro.precio
                    };
                    _context.pedido_encabezado.Add(carrito);
                }
                else
                {
                    carrito.cantidad_libros += 1;
                    carrito.total += libro.precio;
                    _context.pedido_encabezado.Update(carrito);
                }

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CompletarCompra()
        {
            var carrito = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == 1);
            if (carrito != null)
            {
                _context.pedido_encabezado.Remove(carrito);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}

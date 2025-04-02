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

        public IActionResult Index(int idCliente)
        {
            var libros = _context.Libros.ToList();
            var carrito = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == idCliente);

            var viewModel = new CarritoViewModel
            {
                Libros = libros,
                CantidadLibros = carrito?.cantidad_libros ?? 0,
                Total = carrito?.total ?? 0,
                IdCliente = idCliente  
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult AgregarAlCarrito(int idLibro, int idCliente)
        {
            var libro = _context.Libros.FirstOrDefault(l => l.id == idLibro);
            if (libro != null)
            {
                var carrito = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == idCliente);

                if (carrito == null)
                {
                    carrito = new pedido_encabezado
                    {
                        id_cliente = idCliente,
                        cantidad_libros = 1,
                        total = libro.precio,
                        estado = "P"
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
            return RedirectToAction("Index", new { idCliente });
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

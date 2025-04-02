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

        /*[HttpPost]
        public IActionResult CompletarCompra()
        {
            var carrito = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == 1);
            if (carrito != null)
            {
                _context.pedido_encabezado.Remove(carrito);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }*/

        public IActionResult CompletarCompra()
        {
            var pedido = new pedido_encabezado
            {
                id_cliente = 1, // Aquí debes obtener el cliente autenticado si es necesario
                cantidad_libros = _context.pedido_detalle.Count(d => d.id_pedido == 1), // Obtiene el número de libros
                total = _context.pedido_detalle.Where(d => d.id_pedido == 1).Sum(d => _context.Libros.FirstOrDefault(l => l.id == d.id_libro).precio), // Suma los precios
                estado = "P" // Pedido en estado Pendiente
            };

            _context.pedido_encabezado.Add(pedido);
            _context.SaveChanges();

            return View("CierreVenta");


        }




        // Método para mostrar la vista de cierre de venta
        public IActionResult CierreVentas()
        {
            var pedido = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == 1 && p.estado == "P"); // Pedido pendiente

            if (pedido == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cliente = _context.clientes.Find(pedido.id_cliente);
            var detalles = _context.pedido_detalle.Where(d => d.id_pedido == pedido.id).ToList();

            var viewModel = new CierreVentaViewModel
            {
                Cliente = new ClienteViewModel
                {
                    Nombre = cliente.nombre,
                    Email = cliente.email,
                    Direccion = cliente.direccion
                },
                Carrito = detalles.Select(d => new CarritoItemViewModel
                {
                    Nombre = _context.Libros.Find(d.id_libro)?.nombre,
                    Cantidad = 1, // Assuming each detail represents one book
                    Precio = _context.Libros.Find(d.id_libro)?.precio ?? 0
                }).ToList()
            };

            return View(viewModel);
        }


        // Acción para cerrar la venta
        [HttpPost]
        public IActionResult CerrarVenta()
        {
            var pedido = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == 1 && p.estado == "P");
            if (pedido == null)
            {
                return RedirectToAction("Index", "Home");
            }

            pedido.estado = "C"; // CERRADA
            _context.SaveChanges();

            TempData["Mensaje"] = "¡Venta cerrada con éxito!";
            return RedirectToAction("Index", "Home");
        }



    }
}
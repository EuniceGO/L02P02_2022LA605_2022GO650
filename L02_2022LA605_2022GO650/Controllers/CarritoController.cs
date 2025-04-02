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
            return RedirectToAction("Index", new { idCliente });
        }

     

        public IActionResult CompletarCompra(int idCliente)
        {
            var pedido = new pedido_encabezado
            {
                id_cliente = 1, // Aquí debes obtener el cliente autenticado si es necesario
                cantidad_libros = _context.pedido_detalle.Count(d => d.id_pedido == idCliente), // Obtiene el número de libros
                total = _context.pedido_detalle.Where(d => d.id_pedido == idCliente).Sum(d => _context.Libros.FirstOrDefault(l => l.id == d.id_libro).precio), // Suma los precios
                estado = "P" // Pedido en estado Pendiente
            };

            _context.pedido_encabezado.Add(pedido);
            _context.SaveChanges();

            return View("CierreVenta");


        }




        // Método para mostrar la vista de cierre de venta
        public IActionResult CierreVentas(int idCliente)
        {
            var pedido = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == idCliente && p.estado == "P"); // Pedido pendiente

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
        public IActionResult CerrarVenta(int idCliente)
        {
            var pedido = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == idCliente && p.estado == "P");
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
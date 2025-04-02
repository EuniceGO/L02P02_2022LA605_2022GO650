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
                    // Crear un nuevo encabezado de pedido
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

                // Ahora agregar el detalle del libro al carrito
                var detalle = new pedido_detalle
                {
                    id_pedido = carrito.id, // Relacionar el detalle con el encabezado del pedido
                    id_libro = libro.id,    // Libro que se está agregando
                    created_at = DateTime.Now // Establecer la fecha de creación
                };
                _context.pedido_detalle.Add(detalle);

                _context.SaveChanges(); // Guardar cambios
            }

            return RedirectToAction("Index", new { idCliente });
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

        public IActionResult CompletarCompra(int idCliente)
        {
            // Obtener el encabezado del pedido para el cliente
            var pedido = _context.pedido_encabezado.FirstOrDefault(p => p.id_cliente == idCliente);

            if (pedido == null)
            {
                // Si no hay pedido para el cliente, redirigir al carrito
                return RedirectToAction("Index", new { idCliente });
            }

            // Obtener los detalles del pedido
            var carritoItems = _context.pedido_detalle
                                       .Where(d => d.id_pedido == pedido.id)  // Usamos el id del pedido, no el id del cliente
                                       .ToList();

            // Crear un nuevo encabezado de pedido para la compra
            var nuevoPedido = new pedido_encabezado
            {
                id_cliente = idCliente,
                cantidad_libros = carritoItems.Count,
                total = carritoItems.Sum(d => _context.Libros.FirstOrDefault(l => l.id == d.id_libro)?.precio ?? 0),
                estado = "P" // Pedido en estado Pendiente
            };

            // Agregar el encabezado del nuevo pedido
            _context.pedido_encabezado.Add(nuevoPedido);
            _context.SaveChanges();

            // Crear los detalles del nuevo pedido
            foreach (var item in carritoItems)
            {
                var detalle = new pedido_detalle
                {
                    id_pedido = nuevoPedido.id, // Relacionar con el nuevo pedido
                    id_libro = item.id_libro,
                    created_at = DateTime.Now
                };

                _context.pedido_detalle.Add(detalle);
            }

            _context.SaveChanges(); // Guardar los detalles del pedido

            // Redirigir a la vista de cierre de venta
            return RedirectToAction("CierreVentas", new { idCliente });
        }




        public IActionResult CierreVentas(int idCliente)
        {
            // Obtener el pedido con estado pendiente
            var pedido = _context.pedido_encabezado
                                  .FirstOrDefault(p => p.id_cliente == idCliente && p.estado == "P");

            if (pedido == null)
            {
                // Si no se encuentra el pedido, redirigir al inicio
                return RedirectToAction("Index", "Home");
            }

            // Obtener la información del cliente
            var cliente = _context.clientes.Find(pedido.id_cliente);

            // Obtener los detalles del pedido
            var detalles = _context.pedido_detalle.Where(d => d.id_pedido == pedido.id).ToList();

            // Crear el ViewModel con los datos del cliente y carrito
            var viewModel = new CierreVentaViewModel
            {
                Cliente = new ClienteViewModel
                {
                    Nombre = cliente?.nombre ?? "N/A",
                    Email = cliente?.email ?? "N/A",
                    Direccion = cliente?.direccion ?? "N/A"
                },
                // Mapear los detalles del carrito
                Carrito = detalles.GroupBy(d => d.id_libro) // Agrupar por libro
                                  .Select(g => new CarritoItemViewModel
                                  {
                                      Nombre = _context.Libros.FirstOrDefault(l => l.id == g.Key)?.nombre ?? "Desconocido",
                                      Cantidad = g.Count(), // Contar cuántas veces aparece este libro
                                      Precio = _context.Libros.FirstOrDefault(l => l.id == g.Key)?.precio ?? 0
                                  }).ToList(),
                Id_cliente = idCliente // Asegurar que el idCliente se pase a la vista
            };

            return View("CierreVenta", viewModel); // Renderizar la vista CierreVenta
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
            return RedirectToAction("InicioVenta", "Cliente");
        }




    }
}
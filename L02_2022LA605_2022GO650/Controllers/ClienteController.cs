using L02P02_2022LA605_2022GO650.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L02P02_2022LA605_2022GO650.Controllers
{
    public class ClienteController : Controller
    {
        private readonly libreriaDbContext _context;

        public ClienteController(libreriaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InicioVenta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InicioVenta(clientes cliente)
        {
            if (ModelState.IsValid)
            {
                _context.clientes.Add(cliente);
                _context.SaveChanges();

                
                var pedido = new pedido_encabezado
                {
                    id_cliente = cliente.id,
                    cantidad_libros = 0,
                    total = 0,
                    estado = "P"
                };

                _context.pedido_encabezado.Add(pedido);
                _context.SaveChanges();

                return RedirectToAction("ListarLibros", "Libros");
            }

            return View(cliente);
        }
    }
}

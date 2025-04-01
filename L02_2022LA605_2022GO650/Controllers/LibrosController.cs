using Microsoft.AspNetCore.Mvc;

namespace L02P02_2022LA605_2022GO650.Controllers
{
    public class LibrosController : Controller
    {
        public IActionResult Libros()
        {
            return View();
        }
    }
}

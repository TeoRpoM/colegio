using Microsoft.AspNetCore.Mvc;

namespace Clase5.Controllers;

public class TestController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}

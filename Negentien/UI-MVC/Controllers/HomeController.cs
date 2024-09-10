using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NT.UI.MVC.Models;
using UI_MVC.Models;

namespace NT.UI.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Contact(ContactViewModel contactVm)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        return View();
    }

}

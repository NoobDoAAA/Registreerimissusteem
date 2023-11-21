using DataAccessLayer;
using DataAccessLayer.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Controllers.Helpers;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;

        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            using (UritusHandler handler = new(_context))
            {
                var query = handler.GetPlaneeritudUritused();

                query.Wait();

                var model = query.Result.Select(Mapper.MappIt<UritusViewModel>).ToList();

                return View(model);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

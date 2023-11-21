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
                var planeeritud = handler.GetPlaneeritudUritused();

                planeeritud.Wait();

                ViewBag.PlaneeritudUritused = planeeritud.Result.Select(Mapper.MappIt<UritusViewModel>).ToList();

                var moodunud = handler.GetMoodunudUritused();

                moodunud.Wait();

                ViewBag.MoodunudUritused = moodunud.Result.Select(Mapper.MappIt<UritusViewModel>).ToList();

                return View();
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

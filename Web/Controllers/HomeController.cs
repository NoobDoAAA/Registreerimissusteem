using DataAccessLayer;
using DataAccessLayer.dto;
using DataAccessLayer.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Controllers.Helpers;
using Web.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpGet]
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

        [HttpGet]
        public IActionResult PlaneeritudUritused()
        {
            using (UritusHandler handler = new(_context))
            {
                var planeeritud = handler.GetPlaneeritudUritused();

                planeeritud.Wait();

                ViewBag.PlaneeritudUritused = planeeritud.Result.Select(Mapper.MappIt<UritusViewModel>).ToList();
            }

            return PartialView();
        }

        [HttpPost]
        public JsonResult EemaldaUritus(int Id)
        {
            using (UritusHandler handler = new(_context))
            {
                var query = handler.EemaldaUritus(Id);

                query.Wait();

                return Json(new
                {
                    tehtud = query.Result
                });
            }
        }

        [HttpPost]
        public JsonResult LisaUritus(UritusViewModel uritus)
        {
            if (ModelState.IsValid)
            {
                using (UritusHandler handler = new(_context))
                {
                    handler.LisaUritus(Mapper.MappIt<UritusDto>(uritus));

                    return Json(new
                    {
                        tehtud = true
                    });
                }
            }

            return Json(new
            {
                tehtud = false
            });
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
    }
}

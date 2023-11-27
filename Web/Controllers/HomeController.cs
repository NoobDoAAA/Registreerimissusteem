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

        [HttpGet]
        public IActionResult VaataUritus(int Id)
        {
            using (UritusHandler handler = new(_context))
            {
                var uritus = handler.GetUritusById(Id);

                uritus.Wait();

                if (uritus.Result != null)
                {
                    ViewBag.Uritus = Mapper.MappIt<UritusViewModel>(uritus.Result);

                    var eraisikud = handler.GetEraisikOsalejad(Id);

                    eraisikud.Wait();

                    ViewBag.Eraisikud = eraisikud.Result.Select(Mapper.MappIt<EraisikOsalejaViewModel>).ToList();

                    var ettevoted = handler.GetEttevoteOsalejad(Id);

                    ettevoted.Wait();

                    ViewBag.Ettevoted = ettevoted.Result.Select(Mapper.MappIt<EttevoteOsalejaViewModel>).ToList();

                    var makseviisid = handler.GetMakseviisid();

                    makseviisid.Wait();

                    ViewBag.Makseviisid = makseviisid.Result.Select(Mapper.MappIt<MakseviisViewModel>).ToList();
                }
                else
                    ViewBag.Uritus = null;
            }

            return View("Uritus");
        }

        [HttpPost]
        public JsonResult LisaUritus(UritusViewModel uritus)
        {
            if (ModelState.IsValid)
            {
                using (UritusHandler handler = new(_context))
                {
                    var query = handler.LisaUritus(Mapper.MappIt<UritusDto>(uritus));

                    query.Wait();

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

        [HttpPost]
        public JsonResult MuudaUritus(UritusViewModel uritus)
        {
            if (ModelState.IsValid)
            {
                using (UritusHandler handler = new(_context))
                {
                    var query = handler.MuudaUritus(Mapper.MappIt<UritusDto>(uritus));

                    query.Wait();

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

        [HttpPost]
        public IActionResult UrituseAndmed(int Id)
        {
            using (UritusHandler handler = new(_context))
            {
                var uritus = handler.GetUritusById(Id);

                uritus.Wait();

                if (uritus.Result != null) ViewBag.Uritus = Mapper.MappIt<UritusViewModel>(uritus.Result);
                else
                    ViewBag.Uritus = null;
            }

            return PartialView();
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeCare.Models;
using HomeCare.DataAccess.Data.Repository.IRepository;
using HomeCare.Models.View_Models;
using HomeCare.Utility;
using Microsoft.AspNetCore.Http;
using HomeCare.Extensions;

namespace HomeCare.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private HomeViewModel HomeVM;

        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeVM = new HomeViewModel()
            {
                CategoryList = _unitOfWork.Category.GetAll(),
                ServiceList = _unitOfWork.Service.GetAll(includeProperties: "Frequency")
            };

            return View(HomeVM);
        }

        public IActionResult Details(int id)
        {
            var objFromDb = _unitOfWork.Service
                                .GetFirstOrDefault(includeProperties: "Category,Frequency", filter: c => c.Id == id);

            return View(objFromDb);
        }

        public IActionResult AddToCart(int id)
        {
            List<int> sessionList = new List<int>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                sessionList.Add(id);

                HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            }
            else
            {
                // retrieve the session convert it into a list, append that new service id to the list and then store the session back
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

                // f the service id does not exist inside the session list then add it
                if (!sessionList.Contains(id))
                {
                    sessionList.Add(id);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }
            }

            return RedirectToAction(nameof(Index));
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

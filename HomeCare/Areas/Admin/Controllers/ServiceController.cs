using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeCare.DataAccess.Data.Repository.IRepository;
using HomeCare.Models;
using HomeCare.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HomeCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public ServiceViewModel serviceVM { get; set;  }

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            serviceVM = new ServiceViewModel()
            {
                Service = new Models.Service(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown()
            };

            if (id != null)
            {
                serviceVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }

            if (serviceVM.Service == null)
            {
                return NotFound();
            }

            return View(serviceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (serviceVM.Service.Id == 0)
                {
                    // New service being created
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    serviceVM.Service.ImageUrl = @"\images\services\" + fileName + extension;

                    _unitOfWork.Service.Add(serviceVM.Service);
                }
                else
                {
                    // Edit service
                    var serviceFromDb = _unitOfWork.Service.Get(serviceVM.Service.Id);

                    if (files.Count > 0)
                    {
                        // New service being created
                        string fileName = Guid.NewGuid().ToString();
                        var upload = Path.Combine(webRootPath, @"images\services");
                        var newExtension = Path.GetExtension(files[0].FileName);

                        // get path to existing image
                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));

                        // if the image exists, delete it
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        // upload new file
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + newExtension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        serviceVM.Service.ImageUrl = @"\images\services\" + fileName + newExtension;
                    }
                    else
                    {
                        // file does not exist. keep original image
                        serviceVM.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }

                    _unitOfWork.Service.Update(serviceVM.Service);
                }
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                serviceVM.CategoryList = _unitOfWork.Category.GetCategoryListForDropDown();
                serviceVM.FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown();

                return View(serviceVM);
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includeProperties: "Category,Frequency") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Service.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error deleting the service." });
            }

            // get path to existing image
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));

            // if the image exists, delete it
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _unitOfWork.Service.Remove(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Service was deleted successfully." });
        }

        #endregion
    }
}

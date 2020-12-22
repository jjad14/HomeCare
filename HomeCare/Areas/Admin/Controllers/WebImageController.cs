using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeCare.DataAccess.Data;
using HomeCare.DataAccess.Data.Repository.IRepository;
using HomeCare.Models;
using HomeCare.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class WebImageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public WebImageController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            WebImages imageObj = new WebImages();

            if (id == null)
            {
                // return View(imageObj);
            }
            else
            {
                imageObj = _unitOfWork.WebImage.Get(id.GetValueOrDefault());

                if (imageObj == null)
                {
                    return NotFound();
                }
            }

            return View(imageObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, WebImages imageObj)
        {
            if (ModelState.IsValid)
            {
                // any file that has been posted will be inside the files variable
                var files = HttpContext.Request.Form.Files;

                // if image is uploaded convert it to byte arr to store in db
                if (files.Count > 0)
                {
                    byte[] p1 = null;

                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    imageObj.Image = p1;
                }

                // check if uploaded image is for a create or edit operation

                if (imageObj.Id == 0)
                {
                    // add the web image
                    _unitOfWork.WebImage.Add(imageObj);
                }
                else
                {
                    if (files.Count > 0)
                    {
                        // update the web image
                        _unitOfWork.WebImage.Update(imageObj, 1);
                    }
                    else
                    {
                        // update the web image
                        _unitOfWork.WebImage.Update(imageObj, 0);
                    }
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(imageObj);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.WebImage.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.WebImage.Get(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error deleting the image." });
            }

            _unitOfWork.WebImage.Remove(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "The Image was deleted successfully." });
        }

        #endregion
    }
}

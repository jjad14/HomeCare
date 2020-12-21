using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HomeCare.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HomeCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return View(_unitOfWork.User.GetAll(u => u.Id != claim.Value));
        }

        public IActionResult Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _unitOfWork.User.LockUser(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Unlock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _unitOfWork.User.UnlockUser(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

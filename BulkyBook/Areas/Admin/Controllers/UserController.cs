using System.Linq;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.Include(u => u.Company).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach(var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            return Json(new { data = userList });
        }

        // public IActionResult Upsert(int? id)
        // {
        //     Category category = new Category();
        //     if (id == null)
        //     {
        //         // this is for create
        //         return View(category);
        //     }
        //     // this is for edit

        //     category = _unitOfWork.Category.Get(id.GetValueOrDefault());
        //     if (category == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(category);
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public IActionResult Upsert(Category category)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         if (category.Id == 0)
        //         {
        //             _unitOfWork.Category.Add(category);
        //         }
        //         else
        //         {
        //             _unitOfWork.Category.Update(category);
        //         }
        //         _unitOfWork.Save();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(category);
        // }

        // [HttpDelete]
        // public IActionResult Delete(int id)
        // {
        //     var objFromDb = _unitOfWork.Category.Get(id);
        //     if (objFromDb == null)
        //     {
        //         return Json(new { success = false, message = "Error while deleting" });
        //     }
        //     _unitOfWork.Category.Remove(objFromDb);
        //     _unitOfWork.Save();
        //     return Json(new { success = true, message = "Delete Successful" });
        // }
        #endregion
    }
}
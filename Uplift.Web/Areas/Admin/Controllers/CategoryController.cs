using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;

namespace Uplift.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }
            category = _unitOfWork.CategoryRepository.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.CategoryRepository.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.CategoryRepository.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting the category!" });
            }
            _unitOfWork.CategoryRepository.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }

        #endregion
    }
}

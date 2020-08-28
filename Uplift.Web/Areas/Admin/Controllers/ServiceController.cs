using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;
using Uplift.Models.ViewModel;

namespace Uplift.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

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
            ServiceVM serviceVM = new ServiceVM()
            {
                Service = new Service(),
                CategoryList = _unitOfWork.CategoryRepository.GetCategoryForDropDown(),
                FrequencyList = _unitOfWork.FrequencyRepository.GetFrequencyForDropDown()
            };
            if (id == null)
            {
                return View(serviceVM);
            }
            serviceVM.Service = _unitOfWork.ServiceRepository.Get(id.GetValueOrDefault());
            if (serviceVM.Service == null)
            {
                return NotFound();
            }
            else
            {
                return View(serviceVM);
            }

            
        }

        #region API CALLs
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.ServiceRepository.GetAll() });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ServiceVM serviceVM, IFormFile file)
        {
            var files = HttpContext.Request.Form.Files;
            var fileName = file.FileName;
            if (ModelState.IsValid)
            {
                if (serviceVM.Service.Id == 0)
                {
                    // New Service
                    string webHostRoot = _hostEnvironment.WebRootPath;
                    
                }
                else
                {
                    // Update Service
                }

                return RedirectToAction(nameof(Index));
            }
            return View(serviceVM);
        }
        #endregion
    }
}

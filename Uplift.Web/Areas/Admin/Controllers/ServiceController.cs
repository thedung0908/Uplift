using System;
using System.Collections.Generic;
using System.IO;
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
            return Json(new { data = _unitOfWork.ServiceRepository.GetAll(includeProperties : "Category,Frequency") });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ServiceVM serviceVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webHostRoot = _hostEnvironment.WebRootPath;

                var fileName = Guid.NewGuid().ToString() + "_" + files[0].FileName;
                var uploadFolder = Path.Combine(webHostRoot, @"images\services");

                if (serviceVM.Service.Id == 0)
                {

                    // New Service
                    using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    serviceVM.Service.ImageUrl = @"\images\services\" + fileName;
                    _unitOfWork.ServiceRepository.Add(serviceVM.Service);
                }
                else
                {
                    // Update Service
                    if (!System.IO.File.Exists(serviceVM.Service.ImageUrl))
                    {
                        if (serviceVM.Service.ImageUrl != uploadFile)
                        {

                        }

                        using (var fileStream = new FileStream(uploadFile, FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        serviceVM.Service.ImageUrl = uploadFile;
                    }

                    _unitOfWork.ServiceRepository.Update(serviceVM.Service);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(serviceVM);
        }
        #endregion
    }
}

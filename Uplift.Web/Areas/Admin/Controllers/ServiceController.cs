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
                var uploadFolder = Path.Combine(webHostRoot, @"images\services");

                if (serviceVM.Service.Id == 0)
                {
                    // New Service

                    var fileName = Guid.NewGuid().ToString() + "_" + files[0].FileName;
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
                    var objFromDB = _unitOfWork.ServiceRepository.Get(serviceVM.Service.Id);

                    if (files.Count > 0)
                    {
                        //remove old file
                        var imagePath = Path.Combine(webHostRoot, objFromDB.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        //upload new file
                        var fileName = Guid.NewGuid().ToString() + "_" + files[0].FileName;
                        using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        serviceVM.Service.ImageUrl = @"\images\services\" + fileName;
                    }
                    else
                    {
                        serviceVM.Service.ImageUrl = objFromDB.ImageUrl;
                    }

                    _unitOfWork.ServiceRepository.Update(serviceVM.Service);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            serviceVM.CategoryList = _unitOfWork.CategoryRepository.GetCategoryForDropDown();
            serviceVM.FrequencyList = _unitOfWork.FrequencyRepository.GetFrequencyForDropDown();
            return View(serviceVM);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Service service = _unitOfWork.ServiceRepository.Get(id);
            if (service != null)
            {
                string webHostRoot = _hostEnvironment.WebRootPath;
                var imagePath = Path.Combine(webHostRoot, service.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _unitOfWork.ServiceRepository.Remove(service);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Deleted successfully!" });
            }
            return Json(new { success = false, message = "Error while deleting this frequency." });
        }
        #endregion
    }
}

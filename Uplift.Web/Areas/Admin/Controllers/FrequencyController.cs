using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository;
using Uplift.DataAccess.Data.Repository.Interfaces;

namespace Uplift.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var frequencies = _unitOfWork.FrequencyRepository.GetAll();
            return Json(new { frequencies });
        }
        #endregion
    }
}

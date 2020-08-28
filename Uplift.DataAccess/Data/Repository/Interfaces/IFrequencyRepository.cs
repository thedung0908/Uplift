using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface IFrequencyRepository : IRepository<Frequency>
    {
        IEnumerable<SelectListItem> GetFrequencyForDropDown();
        void Update(Frequency frequency);
    }
}

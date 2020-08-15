﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface ICategoryRepository: IRepository<Category>
    {
        IEnumerable<SelectListItem> GetCategoryForDropDown();
        void Update(Category category);
    }
}

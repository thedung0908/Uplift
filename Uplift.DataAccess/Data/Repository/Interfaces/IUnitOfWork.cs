using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        ICategoryRepository categoryRepository { get; }
        void Save();
    }
}

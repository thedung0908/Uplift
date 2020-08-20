using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        IFrequencyRepository FrequencyRepository { get; }
        void Save();
    }
}

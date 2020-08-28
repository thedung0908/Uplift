using System;
using System.Collections.Generic;
using System.Text;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface IServiceRepository : IRepository<Service>
    {
        void Update(Service service);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext _db;
        public FrequencyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Frequency frequency)
        {
            var objFromDB = _db.Frequency.FirstOrDefault(s => s.Id == frequency.Id);
            objFromDB.Name = frequency.Name;
            objFromDB.FrequencyCount = frequency.FrequencyCount;

            _db.SaveChanges();

        }
    }
}

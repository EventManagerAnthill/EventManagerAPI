using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class CompanyRepo : AbstractRepo<Company>, ICompanyRepo
    {
        public CompanyRepo(EventManagerDbContext context)
            :base(context)
        { }

        //public IEnumerable<Company> GetAllCompanies()
        //{
        //    return base.GetAll(x => !x.IsDeleted);
        //}
    }
}

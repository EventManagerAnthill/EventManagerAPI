using Microsoft.EntityFrameworkCore;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class CompanySubRepo : AbstractRepo<CompanySubscription>, ICompanySubRepo
    {
        public CompanySubscription GetCompanySubscription(int companyId)
        {
            var companySub = _eventManagerContext.Set<CompanySubscription>().FirstOrDefault(x => x.CompanyId == companyId);
            return companySub;
        }

        public bool GetStatusOfSubscription(int companyId)
        {          
            var subDt = _eventManagerContext.Set<CompanySubscription>().FirstOrDefault(x => x.CompanyId == companyId);   
            return DateTime.UtcNow.Date <= subDt.SubEndDt;
        }

        public bool GetUserTial(int userId)
        {
            var subTrial = _eventManagerContext.Set<CompanySubscription>().Where(x => x.UserId == userId && x.UseTrialVersion == 2);
            return subTrial != null;
        }
    }
}


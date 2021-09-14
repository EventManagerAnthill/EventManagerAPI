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
            var subDt = _eventManagerContext.Set<CompanySubscription>().Where(x => x.CompanyId == companyId).Select(x => x.SubEndDt.Date);
            
            if (DateTime.UtcNow.Date > Convert.ToDateTime(subDt).Date)
            {
                return false; // if false subscription is finished
            }

            return true;
        }

        public bool GetUserTial(int userId)
        {
            var subTrial = _eventManagerContext.Set<CompanySubscription>().Where(x => x.UserId == userId && x.UseTrialVersion == 2);

            if (subTrial == null)
            {
                return false; //if false user cant user trial version
            }

            return true;
        }
    }
}


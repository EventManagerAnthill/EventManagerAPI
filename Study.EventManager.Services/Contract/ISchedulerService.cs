using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Contract
{
    public interface ISchedulerService: IDisposable
    {
        void Dispose();
        Task ToDoSomething();
        Task CheckFinishedEvents();
    }
}

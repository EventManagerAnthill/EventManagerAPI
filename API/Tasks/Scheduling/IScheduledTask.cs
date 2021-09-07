using System.Threading;
using System.Threading.Tasks;

namespace API.Tasks.Scheduling
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
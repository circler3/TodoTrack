using ForegroundTimeTracker.Models;
using System.Threading.Tasks;

namespace ForegroundTimeTracker
{
    public interface IArrangement
    {
        Task ArrangeAsync();
        bool Enqueue(ProcessPeriod process);
    }
}
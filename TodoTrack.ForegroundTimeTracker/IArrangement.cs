using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace ForegroundTimeTracker
{
    public interface IArrangement
    {
        Task ArrangeAsync();
        bool Enqueue(ProcessPeriod process);
    }
}
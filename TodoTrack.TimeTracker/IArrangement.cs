using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace TimeTracker
{
    public interface IArrangement
    {
        Task ArrangeAsync();
        bool Enqueue(ProcessPeriod process);
    }
}
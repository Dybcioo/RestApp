using System.Threading.Tasks;

namespace TaskApi.Interfaces
{
    public interface IUnitOfWork
    {
        ITaskRepository TaskRepository { get; }
        Task<bool> CompleteAsync();
        bool HasChanges();
    }
}

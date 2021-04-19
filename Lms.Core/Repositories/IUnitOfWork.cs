using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface IUnitOfWork
    {
        ICourseRepository CourseRepository { get; }
        IModuleRepository ModuleRepository { get; }
        Task CompleteAsync();
    }
}

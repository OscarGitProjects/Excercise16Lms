using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    /// <summary>
    /// Interface för Unit of work objektet
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// CourseRepository
        /// </summary>
        ICourseRepository CourseRepository { get; }

        /// <summary>
        /// ModuleRepository
        /// </summary>
        IModuleRepository ModuleRepository { get; }

        /// <summary>
        /// Metod som sparar uppdaterad data i CourseRepository och ModuleRepository
        /// </summary>
        /// <returns>Task</returns>
        Task CompleteAsync();
    }
}

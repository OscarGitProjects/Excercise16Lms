using Lms.Core.Repositories;
using Lms.Data.Data;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    /// <summary>
    /// Unit of work med properties för att anropa repository
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Private field for database context
        /// </summary>
        private readonly ApplicationDbContext m_dbContext;

        /// <summary>
        /// CourseRepository
        /// </summary>
        public ICourseRepository CourseRepository { get; private set; }

        /// <summary>
        /// ModuleRepository
        /// </summary>
        public IModuleRepository ModuleRepository { get; private set; }


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dbContext">Context</param>
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.m_dbContext = dbContext;
            CourseRepository = new CourseRepository(m_dbContext);
            ModuleRepository = new ModuleRepository(m_dbContext);
        }

        /// <summary>
        /// Metod som sparar uppdaterad data i CourseRepository och ModuleRepository
        /// </summary>
        /// <returns>Task</returns>
        public async Task CompleteAsync()
        {
            await CourseRepository.SaveAsync();
            await ModuleRepository.SaveAsync();
        }
    }
}

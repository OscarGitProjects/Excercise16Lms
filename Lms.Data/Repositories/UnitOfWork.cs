using Lms.Core.Repositories;
using Lms.Data.Data;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext m_dbContext;
        public ICourseRepository CourseRepository { get; private set; }
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

        public async Task CompleteAsync()
        {
            await CourseRepository.SaveAsync();
            await ModuleRepository.SaveAsync();
        }
    }
}

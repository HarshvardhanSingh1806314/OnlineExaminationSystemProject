using RESTApi.DataAccess.Repositories.IRepository;

namespace RESTApi.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ITestRepository TestRepository { get; private set; }

        public IQuestionRepository QuestionRepository { get; private set; }

        public IAdminRepository AdminRepository { get; private set; }

        public IStudentRepository StudentRepository { get; private set; }

        public IReportRepository ReportRepository { get; private set; }

        private readonly ApplicationContext _db;

        public UnitOfWork(ApplicationContext db)
        {
            _db = db;
            TestRepository = new TestRepository(_db);
            QuestionRepository = new QuestionRepository(_db);
        }

        public bool Save()
        {
            if(_db.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
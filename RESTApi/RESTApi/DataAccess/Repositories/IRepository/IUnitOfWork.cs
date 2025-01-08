namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        IAdminRepository AdminRepository { get; }

        IStudentRepository StudentRepository { get; }

        ITestRepository TestRepository { get; }

        IQuestionRepository QuestionRepository { get; }

        IReportRepository ReportRepository { get; }

        bool Save();
    }
}

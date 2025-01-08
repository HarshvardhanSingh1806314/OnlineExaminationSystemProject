using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface IStudentRepository : IRepository<Student>
    {
        Student Update(string StudentId, StudentUpdateModel studentUpdateModel);
    }
}

using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface ITestRepository : IRepository<Test>
    {
        Test Update(string TestId, TestAddOrUpdateModel testUpdateModel);
    }
}

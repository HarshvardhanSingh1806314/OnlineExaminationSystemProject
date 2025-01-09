using RESTApi.DataAccess.Repositories.IRepository;
using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories
{
    public class TestRepository : Repository<Test>, ITestRepository
    {
        private readonly ApplicationContext _db;

        public TestRepository(ApplicationContext db) : base(db)
        {
            _db = db;
        }

        public Test Update(string TestId, TestAddOrUpdateModel testUpdateModel)
        {
            Test testExist = _db.Tests.Find(TestId);

            testExist.Name = testUpdateModel.Name != null && testUpdateModel.Name.Length > 0 ? testUpdateModel.Name : testExist.Name;
            testExist.Description = testUpdateModel.Description != null && testUpdateModel.Description.Length > 0 ? testUpdateModel.Description : testExist.Description;
            testExist.Duration = testUpdateModel.Duration > 30 ? testUpdateModel.Duration : testExist.Duration;

            return testExist;
        }
    }
}
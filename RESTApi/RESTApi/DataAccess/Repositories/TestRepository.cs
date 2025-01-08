using RESTApi.DataAccess.Repositories.IRepository;
using RESTApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public Test Update(string TestId, TestUpdateModel testUpdateModel)
        {
            Test testExist = _db.Tests.Find(TestId);
            if(testExist != null)
            {
                testExist.Name = testUpdateModel.Name != null && testUpdateModel.Name.Length > 0 ? testUpdateModel.Name : testExist.Name;
                testExist.Description = testUpdateModel.Description != null && testUpdateModel.Description.Length > 0 ? testUpdateModel.Description : testExist.Description;
            }

            return testExist;
        }
    }
}
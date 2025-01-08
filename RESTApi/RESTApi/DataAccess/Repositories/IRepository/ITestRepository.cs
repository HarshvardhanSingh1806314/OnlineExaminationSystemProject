using RESTApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface ITestRepository : IRepository<Test>
    {
        Test Update(string TestId, TestUpdateModel testUpdateModel);
    }
}

using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Admin Update(int AdminId, AdminAddOrUpdateModel adminUpdateModel);
    }
}

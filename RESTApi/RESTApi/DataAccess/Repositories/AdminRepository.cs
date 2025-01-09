using RESTApi.DataAccess.Repositories.IRepository;
using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly ApplicationContext _db;

        public AdminRepository(ApplicationContext db) : base(db)
        {
            _db = db;
        }

        public Admin Update(int AdminId, AdminAddOrUpdateModel adminUpdateModel)
        {
            Admin adminExist = _db.Admins.Find(AdminId);

            adminExist.AdminId = adminUpdateModel.Id > 100000 ? adminUpdateModel.Id : adminExist.AdminId;

            adminExist.Username = adminUpdateModel.Username != null && adminUpdateModel.Username.Length > 0 ?
                                  adminUpdateModel.Username : adminExist.Username;

            adminExist.EmployeeEmail = adminUpdateModel.EmployeeEmail != null && adminUpdateModel.EmployeeEmail.Length > 0 ?
                                       adminUpdateModel.EmployeeEmail : adminExist.EmployeeEmail;

            adminExist.Password = adminUpdateModel.Password != null && adminUpdateModel.Password.Length > 0 ?
                                  adminUpdateModel.Password : adminExist.Password;

            adminExist.OrganizationName = adminUpdateModel.OrganizationName != null && adminUpdateModel.OrganizationName.Length > 0 ?
                                          adminUpdateModel.OrganizationName : adminExist.OrganizationName;

            adminExist.EmployeeId = adminUpdateModel.EmployeeId > 0 ? adminUpdateModel.EmployeeId : adminExist.EmployeeId;

            return adminExist;
        }
    }
}
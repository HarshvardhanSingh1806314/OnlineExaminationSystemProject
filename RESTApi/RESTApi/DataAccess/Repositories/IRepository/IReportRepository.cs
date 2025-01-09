using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface IReportRepository : IRepository<Report>
    {
        Report Update(string ReportId, ReportAddOrUpdateModel reportUpdateModel);
    }
}

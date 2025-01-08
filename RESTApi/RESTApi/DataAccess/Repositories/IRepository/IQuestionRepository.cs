using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Question Update(string Id, QuestionUpdateModel questionUpdateModel);
    }
}

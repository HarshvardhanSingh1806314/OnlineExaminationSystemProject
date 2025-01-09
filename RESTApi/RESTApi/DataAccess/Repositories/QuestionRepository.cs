using RESTApi.DataAccess.Repositories.IRepository;
using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly ApplicationContext _db;

        public QuestionRepository(ApplicationContext db) : base(db)
        {
            _db = db;
        }

        public Question Update(string Id, QuestionAddOrUpdateModel questionUpdateModel)
        {
            Question questionExist = _db.Questions.Find(Id);

            questionExist.Description = questionUpdateModel.Description != null && questionUpdateModel.Description.Length > 0 ? 
                                        questionUpdateModel.Description : questionExist.Description;

            questionExist.Option1 = questionUpdateModel.Option1 != null && questionUpdateModel.Option1.Length > 0 ? 
                                    questionUpdateModel.Option1 : questionExist.Option1;

            questionExist.Option2 = questionUpdateModel.Option2 != null && questionUpdateModel.Option2.Length > 0 ?
                                    questionUpdateModel.Option2 : questionExist.Option2;

            questionExist.Option3 = questionUpdateModel.Option3 != null && questionUpdateModel.Option3.Length > 0 ?
                                    questionUpdateModel.Option3 : questionExist.Option3;

            questionExist.Option4 = questionUpdateModel.Option4 != null && questionUpdateModel.Option4.Length > 0 ?
                                    questionUpdateModel.Option4 : questionExist.Option4;

            questionExist.Answer = questionUpdateModel.Answer != null && questionUpdateModel.Answer.Length > 0 ?
                                    questionUpdateModel.Answer : questionExist.Answer;

            return questionExist;
        }
    }
}
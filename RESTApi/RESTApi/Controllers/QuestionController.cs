using RESTApi.CustomExceptions;
using RESTApi.DataAccess;
using RESTApi.DataAccess.Repositories;
using RESTApi.DataAccess.Repositories.IRepository;
using RESTApi.Models;
using RESTApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using static RESTApi.Models.CustomModels;

namespace RESTApi.Controllers
{
    [RoutePrefix("api/Question")]
    public class QuestionController : ApiController
    {
        private readonly ApplicationContext _db;

        private readonly IQuestionRepository _questionRepository;

        public QuestionController()
        {
            _db = new ApplicationContext();
            _questionRepository = new QuestionRepository(_db);
        }

        // admin routes
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddQuestion(string testId, [FromBody] QuestionAddOrUpdateModel questionAddModel)
        {
            try
            {
                if(questionAddModel.Description == null ||
                   questionAddModel.Option1 == null ||
                   questionAddModel.Option2 == null ||
                   questionAddModel.Option3 == null ||
                   questionAddModel.Option4 == null ||
                   questionAddModel.Answer == null ||
                   questionAddModel.DifficultyLevel == null ||
                   testId == null)
                {
                    throw new NullEntityException("Question cannot be null");
                }

                if(testId == null)
                {
                    throw new NullReferenceException("TestId cannot be null");
                }

                // extacting current adminID
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);

                // checking if test with the given testId exist or not
                Test test = _db.Tests.Find(testId);
                if(test == null)
                {
                    throw new NullEntityException("The test for which you want to add question does not exist.");
                }

                // checking if the current admin is allowd to add question in the test or not
                if(test.AdminId != adminId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to add question to this test");
                }

                // generating new question id
                string[] options = new string[]
                {
                    questionAddModel.Option1.Trim(),
                    questionAddModel.Option2.Trim(),
                    questionAddModel.Option3.Trim(),
                    questionAddModel.Option4.Trim()
                };
                string questionId = IdGenerator.GenerateIdForQuestions(questionAddModel.Description, options, questionAddModel.Answer);

                // fetching difficulty level for question
                string difficultyLevelId = _db.DifficultyLevels.FirstOrDefault(dfl => dfl.LevelName == questionAddModel.DifficultyLevel.ToUpper()).Id;

                // creating new question
                Question question = new Question
                {
                    Id = questionId,
                    Description = questionAddModel.Description.Trim(),
                    Option1 = questionAddModel.Option1,
                    Option2 = questionAddModel.Option2,
                    Option3 = questionAddModel.Option3,
                    Option4 = questionAddModel.Option4,
                    Answer = questionAddModel.Answer.Trim(),
                    DifficultyLevelId = difficultyLevelId,
                    TestId = testId
                };
                Question newAddedQuestion = _questionRepository.Add(question);
                if(newAddedQuestion != null && _questionRepository.Save())
                {
                    string difficultyLevel = questionAddModel.DifficultyLevel.ToUpper();
                    if(difficultyLevel == StaticDetails.DIFFICULTY_EASY)
                    {
                        test.TotalNoOfEasyQuestions += 1;
                    }
                    else if(difficultyLevel == StaticDetails.DIFFICULTY_MEDIUM)
                    {
                        test.TotalNoOfMediumQuestions += 1;
                    }
                    else if(difficultyLevel == StaticDetails.DIFFICULTY_HARD)
                    {
                        test.TotalNoOfHardQuestions += 1;
                    }
                    test.TotalNoOfQuestions += 1;
                    _db.SaveChanges();
                    return Created("Question", new { 
                        newAddedQuestion.Id,
                        newAddedQuestion.Description,
                        newAddedQuestion.Option1,
                        newAddedQuestion.Option2,
                        newAddedQuestion.Option3,
                        newAddedQuestion.Option4,
                        newAddedQuestion.Answer,
                        newAddedQuestion.DifficultyLevel.LevelName,
                        newAddedQuestion.TestId
                    });
                }
                else
                {
                    throw new OperationFailedException("Question Add Operation Failed");
                }
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NullEntityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Remove")]
        public IHttpActionResult RemoveQuestion(string testId, string questionId)
        {
            try
            {
                if (questionId == null || testId == null)
                {
                    throw new NullReferenceException("Question Id cannot be null");
                }

                // extracting current adminId
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);

                // checking if the question exist or not
                Question question = _questionRepository.Get(q => q.Id == questionId, "Test,DifficultyLevel");
                if (question == null)
                {
                    throw new NullEntityException("Question not found");
                }

                // checking if the question belongs to provided test
                if (question.TestId != testId)
                {
                    throw new ConflictException("Question does not belong to given test");
                }

                // checking if the current admin is authorized to delete question
                if (question.Test.AdminId != adminId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to delete question");
                }

                // updating the test table with the total no of questions in the respective difficulty level and total no of questions
                switch(question.DifficultyLevel.LevelName)
                {
                    case StaticDetails.DIFFICULTY_EASY:
                        question.Test.TotalNoOfEasyQuestions -= 1;
                        break;
                    case StaticDetails.DIFFICULTY_MEDIUM:
                        question.Test.TotalNoOfMediumQuestions -= 1;
                        break;
                    case StaticDetails.DIFFICULTY_HARD:
                        question.Test.TotalNoOfHardQuestions -= 1;
                        break;
                }
                question.Test.TotalNoOfQuestions -= 1;
                _db.SaveChanges();

                // deleting question
                if (_questionRepository.Remove(question) && _questionRepository.Save())
                {
                    return Ok();
                }
                else
                {
                    throw new OperationFailedException("Question Deletion Failed");
                }
            }
            catch(UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ConflictException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NullEntityException)
            {
                return NotFound();
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateQuestion(string testId, string questionId, [FromBody] QuestionAddOrUpdateModel questionUpdateModel)
        {
            try
            {
                if(questionId == null || testId == null)
                {
                    throw new NullReferenceException("Question Id cannot be null");
                }

                // checking if update model has valid difficulty level
                string difficultyLevel = questionUpdateModel.DifficultyLevel?.ToUpper();
                if(difficultyLevel != null && difficultyLevel != StaticDetails.DIFFICULTY_EASY &&
                   difficultyLevel != StaticDetails.DIFFICULTY_MEDIUM &&
                   difficultyLevel != StaticDetails.DIFFICULTY_HARD)
                {
                    throw new InvalidCredentialsException("Incorrect Difficulty Level provided");
                }

                // checking if the question exist or not
                Question questionExist = _questionRepository.Get(q => q.Id == questionId, "Test,DifficultyLevel");
                if(questionExist == null)
                {
                    throw new NullEntityException("Question not found");
                }

                // checking if question belongs to the test whose testId is provided
                if(questionExist.Test.TestId != testId)
                {
                    throw new ConflictException("Question does not belong to test");
                }

                // checking if current admin is authorized to update question
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);
                if(adminId != questionExist.Test.AdminId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to update this question");
                }

                // updating question
                string oldDifficultyLevelName = questionExist.DifficultyLevel.LevelName;
                Question updatedQuestion = _questionRepository.Update(questionId, questionUpdateModel);
                if (updatedQuestion != null && _questionRepository.Save())
                {
                    if(oldDifficultyLevelName != updatedQuestion.DifficultyLevel.LevelName)
                    {
                        string currentDifficultyLevel = updatedQuestion.DifficultyLevel.LevelName;
                        switch(oldDifficultyLevelName)
                        {
                            case StaticDetails.DIFFICULTY_EASY:
                                updatedQuestion.Test.TotalNoOfEasyQuestions -= 1;
                                break;
                            case StaticDetails.DIFFICULTY_MEDIUM:
                                updatedQuestion.Test.TotalNoOfMediumQuestions -= 1;
                                break;
                            case StaticDetails.DIFFICULTY_HARD:
                                updatedQuestion.Test.TotalNoOfHardQuestions -= 1;
                                break;
                        }

                        switch(currentDifficultyLevel)
                        {
                            case StaticDetails.DIFFICULTY_EASY:
                                updatedQuestion.Test.TotalNoOfEasyQuestions += 1;
                                break;
                            case StaticDetails.DIFFICULTY_MEDIUM:
                                updatedQuestion.Test.TotalNoOfMediumQuestions += 1;
                                break;
                            case StaticDetails.DIFFICULTY_HARD:
                                updatedQuestion.Test.TotalNoOfHardQuestions += 1;
                                break;
                        }

                        _db.SaveChanges();
                    }

                    return Ok(new { 
                        updatedQuestion.Id,
                        updatedQuestion.Description,
                        updatedQuestion.Option1,
                        updatedQuestion.Option2,
                        updatedQuestion.Option3,
                        updatedQuestion.Option4,
                        updatedQuestion.Answer,
                        DifficultyLevel = updatedQuestion.DifficultyLevel.LevelName,
                        updatedQuestion.TestId
                    });
                }
                else
                {
                    throw new OperationFailedException("Question Updation Failed");
                }
            }
            catch(OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NullEntityException)
            {
                return NotFound();
            }
            catch(InvalidCredentialsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetQuestionsByTestId")]
        public IHttpActionResult GetAllQuestionsByTestId(string testId)
        {
            try
            {
                if(testId == null)
                {
                    throw new NullReferenceException("TestId cannot be null");
                }

                // fetching all the questions for the given testId
                List<Question> questionList = _questionRepository.GetAll(q => q.TestId == testId, "DifficultyLevel").ToList();
                if(questionList == null || questionList.Count == 0)
                {
                    throw new NullEntityException("Questions not available");
                }

                // creatin the response
                Object[] questionResponseList = new Object[questionList.Count];
                for(int i=0;i<questionList.Count;i++)
                {
                    questionResponseList[i] = new
                    {
                        questionList[i].Id,
                        questionList[i].Description,
                        questionList[i].Option1,
                        questionList[i].Option2,
                        questionList[i].Option3,
                        questionList[i].Option4,
                        questionList[i].Answer,
                        DifficultyLevel = questionList[i].DifficultyLevel.LevelName,
                        questionList[i].TestId
                    };
                }

                return Ok(questionResponseList);
            }
            catch(NullEntityException)
            {
                return NotFound();
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetQuestionsByDifficultyLevel")]
        public IHttpActionResult GetAllQuestionsByDifficultyLevel(string difficultyLevel)
        {
            try
            {
                List<Question> questionList = null;
                string difficultyLevelId = null;
                switch(difficultyLevel.ToUpper())
                {
                    case StaticDetails.DIFFICULTY_EASY:
                        difficultyLevelId = _db.DifficultyLevels.FirstOrDefault(dfl => dfl.LevelName == StaticDetails.DIFFICULTY_EASY).Id;
                        questionList = _questionRepository.GetAll(q => q.DifficultyLevelId == difficultyLevelId, "DifficultyLevel").ToList();
                        break;
                    case StaticDetails.DIFFICULTY_MEDIUM:
                        difficultyLevelId = _db.DifficultyLevels.FirstOrDefault(dfl => dfl.LevelName == StaticDetails.DIFFICULTY_MEDIUM).Id;
                        questionList = _questionRepository.GetAll(q => q.DifficultyLevelId == difficultyLevelId, "DifficultyLevel").ToList();
                        break;
                    case StaticDetails.DIFFICULTY_HARD:
                        difficultyLevelId = _db.DifficultyLevels.FirstOrDefault(dfl => dfl.LevelName == StaticDetails.DIFFICULTY_HARD).Id;
                        questionList = _questionRepository.GetAll(q => q.DifficultyLevelId == difficultyLevelId, "DifficultyLevel").ToList();
                        break;
                }

                if(questionList == null)
                {
                    throw new NullEntityException("Questions Not Found");
                }

                Object[] questionResponseList = new Object[questionList.Count];
                for(int i=0;i<questionList.Count;i++)
                {
                    questionResponseList[i] = new
                    {
                        questionList[i].Id,
                        questionList[i].Description,
                        questionList[i].Option1,
                        questionList[i].Option2,
                        questionList[i].Option3,
                        questionList[i].Option4,
                        questionList[i].Answer,
                        questionList[i].DifficultyLevel.LevelName
                    };
                }

                return Ok(questionList);
            }
            catch(NullEntityException)
            {
                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetQuestionsByTestIdAndDifficultyLevel")]
        public IHttpActionResult GetQuestionsByTestIdAndDifficultyLevel(string testId, string difficultyLevel)
        {
            try
            {
                if (testId == null || testId.Length == 0 || difficultyLevel == null || difficultyLevel.Length == 0)
                {
                    throw new NullReferenceException("TestId and Difficulty Level cannot be null");
                }

                // fetching the questions based on testId and difficulty level
                List<Question> questionList = null;
                string difficultyLevelId = null;
                switch (difficultyLevel.ToUpper())
                {
                    case StaticDetails.DIFFICULTY_EASY:
                        difficultyLevelId = _db.DifficultyLevels.FirstOrDefault(dfl => dfl.LevelName == StaticDetails.DIFFICULTY_EASY).Id;
                        questionList = _questionRepository.GetAll(q => q.DifficultyLevelId == difficultyLevelId && q.TestId == testId, "DifficultyLevel").ToList();
                        break;
                    case StaticDetails.DIFFICULTY_MEDIUM:
                        difficultyLevelId = _db.DifficultyLevels.FirstOrDefault(dfl => dfl.LevelName == StaticDetails.DIFFICULTY_MEDIUM).Id;
                        questionList = _questionRepository.GetAll(q => q.DifficultyLevelId == difficultyLevelId && q.TestId == testId, "DifficultyLevel").ToList();
                        break;
                    case StaticDetails.DIFFICULTY_HARD:
                        difficultyLevelId = _db.DifficultyLevels.FirstOrDefault(dfl => dfl.LevelName == StaticDetails.DIFFICULTY_HARD).Id;
                        questionList = _questionRepository.GetAll(q => q.DifficultyLevelId == difficultyLevelId && q.TestId == testId, "DifficultyLevel").ToList();
                        break;
                }

                if(questionList == null)
                {
                    throw new NullEntityException("Not Found");
                }

                Object[] questionResponseList = new Object[questionList.Count];
                for (int i = 0; i < questionList.Count; i++)
                {
                    questionResponseList[i] = new
                    {
                        questionList[i].Id,
                        questionList[i].Description,
                        questionList[i].Option1,
                        questionList[i].Option2,
                        questionList[i].Option3,
                        questionList[i].Option4,
                        DifficultyLevel = questionList[i].DifficultyLevel.LevelName,
                        questionList[i].TestId
                    };
                }

                return Ok(questionResponseList);
            }
            catch(NullEntityException)
            {
                return NotFound();
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetQuestionById")]
        public IHttpActionResult GetQuestionById(string questionId)
        {
            try
            {
                if(questionId == null || questionId.Length == 0)
                {
                    throw new NullReferenceException("QuestionID cannot be null");
                }

                // fetching the question
                Question question = _questionRepository.Get(q => q.Id == questionId, "DifficultyLevel");
                if(question == null)
                {
                    throw new NullEntityException("Not Found");
                }

                return Ok(new
                {
                    question.Description,
                    question.Option1,
                    question.Option2,
                    question.Option3,
                    question.Option4,
                    question.Answer,
                    DifficultyLevel = question.DifficultyLevel.LevelName,
                });
            }
            catch(NullEntityException)
            {
                return NotFound();
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

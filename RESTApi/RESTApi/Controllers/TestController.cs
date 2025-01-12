using RESTApi.CustomExceptions;
using RESTApi.DataAccess;
using RESTApi.DataAccess.Repositories;
using RESTApi.DataAccess.Repositories.IRepository;
using RESTApi.Models;
using RESTApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using static RESTApi.Models.CustomModels;

namespace RESTApi.Controllers
{
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        private readonly ITestRepository _testRepository;

        private readonly ApplicationContext _db;

        public TestController()
        {
            _db = new ApplicationContext();
            _testRepository = new TestRepository(_db);
        }

        // admin routes
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddTest([FromBody] TestAddOrUpdateModel testAddModel)
        {
            try
            {
                // checking if test information provided is null or not
                if(testAddModel.Name == null || testAddModel.Description == null || testAddModel.Duration < 30)
                {
                    throw new NullEntityException("Invalid Test Information");
                }

                // extracting id of admin who created the test
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);

                // checking if test already exist
                Test testExist = _testRepository.Get(t => t.Name == testAddModel.Name && t.AdminId == adminId);
                if(testExist != null)
                {
                    throw new EntityAlreadyExistException($"Test with name: {testAddModel.Name} already exist");
                }

                // creating new test id
                string testId = IdGenerator.GenerateIdForTests(testAddModel.Name, testAddModel.Description);


                // creating new test entity
                Test test = new Test
                {
                    TestId = testId,
                    Name = testAddModel.Name,
                    Description = testAddModel.Description,
                    Duration = testAddModel.Duration,
                    AdminId = adminId
                };

                Test newAddedTest = _testRepository.Add(test);
                if (newAddedTest != null && _testRepository.Save())
                {
                    return Created("Test", new { 
                        test.Name,
                        test.Description,
                        test.Duration,
                        test.TotalNoOfEasyQuestions,
                        test.TotalNoOfMediumQuestions,
                        test.TotalNoOfHardQuestions,
                        test.TotalNoOfQuestions
                    });
                }
                else
                {
                    throw new OperationFailedException("Test Cannot be Created");
                }
            }
            catch(OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NullEntityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(EntityAlreadyExistException ex)
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
        public IHttpActionResult RemoveTest(string testId)
        {
            try
            {
                if(testId == null)
                {
                    throw new NullReferenceException("Test Id cannot be null");
                }

                // checking if test with the given testId exist or not
                Test testExist = _testRepository.Get(t => t.TestId == testId);
                if(testExist == null)
                {
                    throw new NullEntityException("Test does not exist");
                }

                // extracting admin id and checking if the current admin is authorized to delete this test or not
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);
                if(adminId != testExist.AdminId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to remove this test");
                }

                // removing the test
                bool isTestRemoved = _testRepository.Remove(testExist);
                if(isTestRemoved && _testRepository.Save())
                {
                    return Ok();
                }
                else
                {
                    throw new OperationFailedException("Test cannot be removed");
                }
            }
            catch(NullEntityException)
            {
                return NotFound();
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateTest(string testId, [FromBody] TestAddOrUpdateModel testUpdateModel)
        {
            try
            {
                if(testId == null)
                {
                    throw new NullReferenceException("Test Id cannot be null");
                }

                // checking if test with testId exist or not
                Test testExist = _testRepository.Get(t => t.TestId == testId);
                if(testExist == null)
                {
                    throw new NullEntityException($"Test with id: {testId} does not exist");
                }

                // checking if the current admin is authorized to update the test or not
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);
                if(adminId != testExist.AdminId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to delete this test");
                }

                Test updatedTest = _testRepository.Update(testId, testUpdateModel);
                if(updatedTest != null && _testRepository.Save())
                {
                    return Ok(updatedTest);
                }
                else
                {
                    throw new OperationFailedException("Test Updation Failed");
                }
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
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllTests")]
        public IHttpActionResult GetAllTest()
        {
            try
            {
                ClaimsPrincipal user = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(user.Claims.ElementAt(1).Value);

                // fetching all the test that current admin has created
                List<Test> testList = _testRepository.GetAll(t => t.AdminId == adminId).ToList();
                if (testList == null || testList.Count == 0)
                {
                    throw new NullEntityException("No Tests Available");
                }

                return Ok(testList);
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

        // user routes
        [HttpGet]
        [Route("GetTestsByOrgName")]
        public IHttpActionResult GetTestByOrgName(string organizationName)
        {
            try
            {
                if(organizationName == null || organizationName.Length == 0)
                {
                    throw new NullReferenceException("Organization Name cannot be null");
                }

                // fetching all the tests created by given organization name
                List<Test> tests = _testRepository.GetAll(t => t.Admin.OrganizationName == organizationName).ToList();
                if(tests == null)
                {
                    throw new NullEntityException("Tests not found");
                }


                // creating response
                Object[] testList = new Object[tests.Count];
                for(int i=0;i<tests.Count;i++)
                {
                    testList[i] = new
                    {
                        tests[i].TestId,
                        tests[i].Name,
                        tests[i].Duration
                    };
                }

                return Ok(testList);
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
        [Route("GetTestById")]
        public IHttpActionResult GetTestById(string testId)
        {
            try
            {
                if(testId == null || testId.Length == 0)
                {
                    throw new NullReferenceException("TestId cannot be null");
                }

                // fetching test
                Test test = _testRepository.Get(t => t.TestId == testId, "Questions");
                if(test == null)
                {
                    throw new NullEntityException("Test not found");
                }

                // creating questions object array
                Object[] questions = new Object[test.Questions.Count];
                for(int i=0;i<test.Questions.Count;i++)
                {
                    questions[i] = new
                    {
                        test.Questions.ElementAt(i).Description,
                        test.Questions.ElementAt(i).Option1,
                        test.Questions.ElementAt(i).Option2,
                        test.Questions.ElementAt(i).Option3,
                        test.Questions.ElementAt(i).Option4,
                        test.Questions.ElementAt(i).DifficultyLevel.LevelName
                    };
                }

                return Ok(new { 
                    test.TestId,
                    test.Name,
                    test.Description,
                    test.Duration,
                    Questions = questions
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

        [HttpPost]
        [Route("SubmitTest")]
        public IHttpActionResult SubmitTest(string testId, [FromBody] SubmitTestModel submitTestModel)
        {
            try
            {
                if(testId == null || testId.Length == 0)
                {
                    throw new NullReferenceException("Test Id cannot be null");
                }

                // extracting studentId
                ClaimsPrincipal student = HttpContext.Current.User as ClaimsPrincipal;
                string studentId = student.Claims.ElementAt(1).Value;

                // checking if test wih testId exist or not
                Test testExist = _testRepository.Get(t => t.TestId == testId, "Questions");
                if(testExist == null)
                {
                    throw new NullEntityException("Test does not exist");
                }

                // checking if student with studentId exist or not
                Student studentExist = _db.Students.Find(studentId);
                if(studentExist == null)
                {
                    throw new NullEntityException("Student doed not exist");
                }

                // creating a report object
                String reportId = IdGenerator.GenerateIdForReports(studentId, testId);
                Report testReport = new Report
                {
                    Id = reportId,
                    StudentId = studentId,
                    TestId = testId
                };

                // evaluating easy level questions
                foreach(QuestionResponse response in submitTestModel.EasyLevel)
                {
                    if(response.Answer != null && response.Answer.Length > 0)
                    {
                        testReport.TotalAttemptsInEasyQuestions += 1;
                        if(response.Answer == testExist.Questions.FirstOrDefault(q => q.Id == response.QuestionId).Answer)
                        {
                            testReport.CorrectAttempsInEasyQuestions += 1;
                        }
                    }
                }

                // evaluating medium level questions
                foreach(QuestionResponse response in submitTestModel.MediumLevel) {
                    if(response.Answer != null && response.Answer.Length > 0)
                    {
                        testReport.TotalAttemptsInMediumQuestions += 1;
                        if(response.Answer == testExist.Questions.FirstOrDefault(q => q.Id == response.QuestionId).Answer)
                        {
                            testReport.CorrectAttemptsInMediumQuestions += 1;
                        }
                    }
                }

                // evaluating hard level questions
                foreach(QuestionResponse response in submitTestModel.HardLevel)
                {
                    if(response.Answer != null && response.Answer.Length > 0)
                    {
                        testReport.TotalAttemptsInHardQuestions += 1;
                        if(response.Answer == testExist.Questions.FirstOrDefault(q => q.Id == response.QuestionId).Answer)
                        {
                            testReport.CorrectAttemptsInHardQuestions += 1;
                        }
                    }
                }

                if(testReport.CorrectAttempsInEasyQuestions >= Math.Ceiling(testExist.TotalNoOfEasyQuestions / 2.0) &&
                   testReport.CorrectAttemptsInMediumQuestions >= Math.Ceiling(testExist.TotalNoOfMediumQuestions / 2.0) &&
                   testReport.CorrectAttemptsInHardQuestions >= Math.Ceiling(testExist.TotalNoOfHardQuestions / 2.0))
                {
                    testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_PASSED).ResultId;
                }
                else
                {
                    testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_FAILED).ResultId;
                }

                // adding report to database
                if(_db.Reports.Add(testReport) != null && _db.SaveChanges() > 0)
                {
                    return Ok(new { 
                        StudentName = testReport.Student.Username,
                        TestName = testReport.Test.Name,
                        testReport.Test.Admin.OrganizationName,
                        TestDuration = testReport.Test.Duration,
                        CorrectAttemptsInEasyQuestions = testReport.CorrectAttempsInEasyQuestions,
                        testReport.TotalAttemptsInEasyQuestions,
                        testReport.Test.TotalNoOfEasyQuestions,
                        testReport.CorrectAttemptsInMediumQuestions,
                        testReport.TotalAttemptsInMediumQuestions,
                        testReport.Test.TotalNoOfMediumQuestions,
                        testReport.CorrectAttemptsInHardQuestions,
                        testReport.TotalAttemptsInHardQuestions,
                        testReport.Test.TotalNoOfHardQuestions,
                        Result = testReport.Result.Name
                    });
                }
                else
                {
                    throw new OperationFailedException("Test Evaluation Failed");
                }
            }
            catch(OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NullEntityException ex)
            {
                return BadRequest(ex.Message);
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

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
                Test testExist = _testRepository.Get(t => t.Name == testAddModel.Name.Trim() && t.AdminId == adminId);
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
                    Name = testAddModel.Name.Trim(),
                    Description = testAddModel.Description.Trim(),
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
                List<Test> tests = _testRepository.GetAll(t => t.AdminId == adminId).ToList();
                if (tests == null || tests.Count == 0)
                {
                    throw new NullEntityException("No Tests Available");
                }

                Object[] testList = new object[tests.Count];
                for (int i = 0; i < tests.Count; i++)
                {
                    testList[i] = new
                    {
                        tests[i].Name,
                        tests[i].Description,
                        tests[i].TotalNoOfEasyQuestions,
                        tests[i].TotalNoOfMediumQuestions,
                        tests[i].TotalNoOfHardQuestions,
                        tests[i].TotalNoOfQuestions,
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
        [Route("GetAllTestAndTestIdList")]
        public IHttpActionResult GetAllTestAndTestIdList()
        {
            try
            {
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);

                // fetching all the tests with only name and id
                var testList = _testRepository.GetAll(t => t.AdminId == adminId).Select(test => new { test.Name, test.TestId}).ToList();
                if(testList == null || testList.Count == 0)
                {
                    throw new NullEntityException("No Tests Available");
                }

                return Ok(testList);
            }
            catch(NullEntityException ex)
            {
                return NotFound();
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

        // student route
        [HttpGet]
        [Route("GetTests")]
        public IHttpActionResult GetTests()
        {
            try
            {
                List<Test> tests = _testRepository.GetAll().ToList();
                List<object> randomTests = new List<object>();
                if(tests.Count == 0)
                {
                    throw new NullEntityException("Not Found");
                }
                
                // if there are more than 6 tests present in database then it will send back random 6 tests
                if(tests.Count > 6)
                {
                    int randomIndex = 0;
                    Random randomIndexGenerator = new Random();
                    while(randomTests.Count < 6)
                    {
                        randomIndex = randomIndexGenerator.Next(0, tests.Count - 1);
                        randomTests.Add(new
                        {
                            tests[randomIndex].TestId,
                            tests[randomIndex].Name,
                            tests[randomIndex].Duration
                        });
                        tests.RemoveAt(randomIndex);
                    }
                }
                // otherwise it will send the existing tests
                else
                {
                    foreach(Test test in tests)
                    {
                        randomTests.Add(new
                        {
                            test.TestId,
                            test.Name,
                            test.Duration
                        });
                    }
                }

                return Ok(randomTests);
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

        [HttpPost]
        [Route("SubmitTest")]
        public IHttpActionResult SubmitTest(string testId, string difficultyLevel, [FromBody] SubmitTestModel submitTestModel)
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
                Report testReport = _db.Reports.FirstOrDefault(r => r.StudentId == studentId && r.TestId == testId);
                if(testReport == null)
                {
                    string reportId = IdGenerator.GenerateIdForReports(studentId, testId);
                    testReport = new Report
                    {
                        Id = reportId,
                        StudentId = studentId,
                        TestId = testId
                    };
                }

                // evaluating questions
                bool passed = false;
                int correctAttemtps = 0;
                int totalAttempts = 0;
                int totalQuestions = 0;
                switch (difficultyLevel.ToUpper())
                {
                    case StaticDetails.DIFFICULTY_EASY:
                        totalQuestions = testExist.TotalNoOfEasyQuestions;
                        foreach(QuestionResponse response in submitTestModel.QuestionResponses)
                        {
                            if(response.Answer != null && response.Answer.Length > 0)
                            {
                                testReport.TotalAttemptsInEasyQuestions += 1;
                                totalAttempts++;
                                if(response.Answer.Trim().ToLower() == testExist.Questions.FirstOrDefault(q => q.Id == response.QuestionId).Answer.ToLower())
                                {
                                    testReport.CorrectAttempsInEasyQuestions += 1;
                                    correctAttemtps++;
                                }
                            }
                        }
                        if(testReport.CorrectAttempsInEasyQuestions >= Math.Ceiling(testExist.TotalNoOfEasyQuestions / 2.0))
                        {
                            testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_PASSED).ResultId;
                            passed = true;
                        }
                        else
                        {
                            testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_FAILED).ResultId;
                        }
                        if (_db.Reports.Add(testReport) == null)
                        {
                            throw new OperationFailedException("Something went wrong during evaluation");
                        }
                        break;
                    case StaticDetails.DIFFICULTY_MEDIUM:
                        totalQuestions = testExist.TotalNoOfMediumQuestions;
                        foreach(QuestionResponse response in submitTestModel.QuestionResponses)
                        {
                            if(response.Answer != null && response.Answer.Length > 0)
                            {
                                testReport.TotalAttemptsInMediumQuestions += 1;
                                totalAttempts++;
                                if(response.Answer.Trim().ToLower() == testExist.Questions.FirstOrDefault(q => q.Id == response.QuestionId).Answer.ToLower())
                                {
                                    testReport.CorrectAttemptsInMediumQuestions += 1;
                                    correctAttemtps++;
                                }
                            }
                        }
                        if(testReport.CorrectAttemptsInMediumQuestions >= Math.Ceiling(testExist.TotalNoOfMediumQuestions / 2.0))
                        {
                            testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_PASSED).ResultId;
                            passed = true;
                        }
                        else
                        {
                            testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_FAILED).ResultId;
                        }
                        break;
                    case StaticDetails.DIFFICULTY_HARD:
                        totalQuestions = testExist.TotalNoOfHardQuestions;
                        foreach (QuestionResponse response in submitTestModel.QuestionResponses)
                        {
                            if (response.Answer != null && response.Answer.Length > 0)
                            {
                                testReport.TotalAttemptsInHardQuestions += 1;
                                totalAttempts++;
                                if (response.Answer.Trim().ToLower() == testExist.Questions.FirstOrDefault(q => q.Id == response.QuestionId).Answer.ToLower())
                                {
                                    testReport.CorrectAttemptsInHardQuestions += 1;
                                    correctAttemtps++;
                                }
                            }
                        }
                        if(testReport.CorrectAttemptsInHardQuestions >= Math.Ceiling(testExist.TotalNoOfHardQuestions / 2.0))
                        {
                            testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_PASSED).ResultId;
                            passed = true;
                        }
                        else
                        {
                            testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_FAILED).ResultId;
                        }
                        break;
                }
                if (_db.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        StudentName = studentExist.Username,
                        TestName = testExist.Name,
                        DifficultyLevel = difficultyLevel.ToUpper(),
                        CorrectAttempts = correctAttemtps,
                        TotalAttempts = totalAttempts,
                        TotalQuestions = totalQuestions,
                        TestResult = passed ? StaticDetails.RESULT_PASSED : StaticDetails.RESULT_FAILED,
                    });
                }
                else
                {
                    throw new OperationFailedException("Test Evaluation Failed");
                }

                // evaluating easy level questions
                //foreach(QuestionResponse response in submitTestModel.EasyLevel)
                //{
                //    if(response.Answer != null && response.Answer.Length > 0)
                //    {
                //        testReport.TotalAttemptsInEasyQuestions += 1;
                //        if(response.Answer == testExist.Questions.FirstOrDefault(q => q.Id == response.QuestionId).Answer)
                //        {
                //            testReport.CorrectAttempsInEasyQuestions += 1;
                //        }
                //    }
                //}

                // evaluating medium level questions
                //foreach(QuestionResponse response in submitTestModel.MediumLevel) {
                //    if(response.Answer != null && response.Answer.Length > 0)
                //    {
                //        testReport.TotalAttemptsInMediumQuestions += 1;
                //        if(response.Answer == testExist.Questions.FirstOrDefault(q => q.Id == response.QuestionId).Answer)
                //        {
                //            testReport.CorrectAttemptsInMediumQuestions += 1;
                //        }
                //    }
                //}

                // evaluating hard level questions
                //foreach(QuestionResponse response in submitTestModel.HardLevel)
                //{
                //    if(response.Answer != null && response.Answer.Length > 0)
                //    {
                //        testReport.TotalAttemptsInHardQuestions += 1;
                //        if(response.Answer == testExist.Questions.FirstOrDefault(q => q.Id == response.QuestionId).Answer)
                //        {
                //            testReport.CorrectAttemptsInHardQuestions += 1;
                //        }
                //    }
                //}

                //if(testReport.CorrectAttempsInEasyQuestions >= Math.Ceiling(testExist.TotalNoOfEasyQuestions / 2.0) &&
                //   testReport.CorrectAttemptsInMediumQuestions >= Math.Ceiling(testExist.TotalNoOfMediumQuestions / 2.0) &&
                //   testReport.CorrectAttemptsInHardQuestions >= Math.Ceiling(testExist.TotalNoOfHardQuestions / 2.0))
                //{
                //    testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_PASSED).ResultId;
                //}
                //else
                //{
                //    testReport.ResultId = _db.Results.FirstOrDefault(r => r.Name == StaticDetails.RESULT_FAILED).ResultId;
                //}

                // adding report to database
                //if (_db.Reports.Add(testReport) != null && _db.SaveChanges() > 0)
                //{
                //    return Ok(new { 
                //        StudentName = testReport.Student.Username,
                //        TestName = testReport.Test.Name,
                //        testReport.Test.Admin.OrganizationName,
                //        TestDuration = testReport.Test.Duration,
                //        CorrectAttemptsInEasyQuestions = testReport.CorrectAttempsInEasyQuestions,
                //        testReport.TotalAttemptsInEasyQuestions,
                //        testReport.Test.TotalNoOfEasyQuestions,
                //        testReport.CorrectAttemptsInMediumQuestions,
                //        testReport.TotalAttemptsInMediumQuestions,
                //        testReport.Test.TotalNoOfMediumQuestions,
                //        testReport.CorrectAttemptsInHardQuestions,
                //        testReport.TotalAttemptsInHardQuestions,
                //        testReport.Test.TotalNoOfHardQuestions,
                //        Result = testReport.Result.Name
                //    });
                //}
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

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

                // checking if test already exist
                Test testExist = _testRepository.Get(t => t.Name == testAddModel.Name);
                if(testExist != null)
                {
                    throw new EntityAlreadyExistException($"Test with name: {testAddModel.Name} already exist");
                }

                // creating new test id
                string testId = IdGenerator.GenerateIdForTests(testAddModel.Name, testAddModel.Description);

                // extracting id of admin who created the test
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);

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
                    return Created("Test", test);
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
            catch(NullEntityException ex)
            {
                return NotFound();
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UnauthorizedAccessException ex)
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
            catch(NullEntityException ex)
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
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllTest")]
        public IHttpActionResult GetAllTest()
        {
            try
            {
                ClaimsPrincipal user = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(user.Claims.ElementAt(1).Value);

                // fetching all the test that current admin has created
                List<Test> testList = _testRepository.GetAll(t => t.AdminId == adminId).ToList();
                if(testList == null)
                {
                    throw new NullEntityException("No Tests Available");
                }

                return Ok(testList);
            }
            catch(NullEntityException ex)
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

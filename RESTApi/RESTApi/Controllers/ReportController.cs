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
    [RoutePrefix("api/Report")]
    public class ReportController : ApiController
    {
        private readonly ApplicationContext _db;

        private readonly IReportRepository _reportRepository;

        public ReportController()
        {
            _db = new ApplicationContext();
            _reportRepository = new ReportRepository(_db);
        }

        // admin and student route
        [HttpGet]
        [Route("GetReportsByTestName")]
        public IHttpActionResult GetReportsByTestName(string testName)
        {
            try
            {
                if(testName == null || testName.Trim().Length == 0)
                {
                    throw new NullReferenceException("Test Id cannot be null");
                }

                // extracting userId and role
                ClaimsPrincipal user = HttpContext.Current.User as ClaimsPrincipal;
                string userRole = user.Claims.ElementAt(0).Value;
                string userId = user.Claims.ElementAt(1).Value;

                // fetching all the reports for the give testName and userId
                List<Report> reportList = null;
                if(userRole == StaticDetails.ROLE_ADMIN)
                {
                    int adminId = int.Parse(userId);
                    reportList = _reportRepository.GetAll(r => r.Test.Name == testName && r.Test.AdminId == adminId, "Student,Test,Result").ToList();
                }
                else if(userRole == StaticDetails.ROLE_STUDENT)
                {
                    reportList = _reportRepository.GetAll(r => r.Test.Name == testName && r.StudentId == userId, "Student,Test,Result").ToList();
                }

                if(reportList == null)
                {
                    throw new NullEntityException("Reports Not Found");
                }

                // creating response
                Object[] reportResponseList = new Object[reportList.Count];
                for(int i=0;i<reportList.Count;i++)
                {
                    reportResponseList[i] = new
                    {
                        StudentName = reportList[i].Student.Username,
                        TestName = reportList[i].Test.Name,
                        TestDuration = reportList[i].Test.Duration,
                        CorrectAttemptsInEasyQuestions = reportList[i].CorrectAttempsInEasyQuestions,
                        reportList[i].TotalAttemptsInEasyQuestions,
                        reportList[i].Test.TotalNoOfEasyQuestions,
                        reportList[i].CorrectAttemptsInMediumQuestions,
                        reportList[i].TotalAttemptsInMediumQuestions,
                        reportList[i].Test.TotalNoOfMediumQuestions,
                        reportList[i].CorrectAttemptsInHardQuestions,
                        reportList[i].TotalAttemptsInHardQuestions,
                        reportList[i].Test.TotalNoOfHardQuestions,
                        Result = reportList[i].Result.Name
                    };
                }

                return Ok(reportResponseList);
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

        // route for both admin and student
        [HttpGet]
        [Route("GetAllReports")]
        public IHttpActionResult GetAllReports()
        {
            try
            {
                // extracting user id and role
                ClaimsPrincipal user = HttpContext.Current.User as ClaimsPrincipal;
                string userId = user.Claims.ElementAt(1).Value;
                string userRole = user.Claims.ElementAt(0).Value;

                List<Report> reportList = null;

                /* if role is admin then we will return all the reports for the tests where
                    AdminId == userId
                 */
                if(userRole == StaticDetails.ROLE_ADMIN)
                {
                    int adminId = int.Parse(userId);
                    reportList = _reportRepository.GetAll(r => r.Test.AdminId == adminId, "Student,Test,Result").ToList();
                }
                else if(userRole == StaticDetails.ROLE_STUDENT)
                {
                    reportList = _reportRepository.GetAll(r => r.StudentId == userId, "Student,Test,Result").ToList();
                }

                if(reportList == null)
                {
                    throw new NullEntityException("No Reports Found");
                }

                Object[] reportResponseList = new Object[reportList.Count];
                // creating response
                for(int i=0;i<reportList.Count;i++)
                {
                    reportResponseList[i] = new
                    {
                        reportList[i].Student.Username,
                        TestName = reportList[i].Test.Name,
                        reportList[i].Test.Admin.OrganizationName,
                        TestDuration = reportList[i].Test.Duration,
                        CorrectAttemptsInEasyQuestions = reportList[i].CorrectAttempsInEasyQuestions,
                        reportList[i].TotalAttemptsInEasyQuestions,
                        reportList[i].Test.TotalNoOfEasyQuestions,
                        reportList[i].CorrectAttemptsInMediumQuestions,
                        reportList[i].TotalAttemptsInMediumQuestions,
                        reportList[i].Test.TotalNoOfMediumQuestions,
                        reportList[i].CorrectAttemptsInHardQuestions,
                        reportList[i].TotalAttemptsInHardQuestions,
                        reportList[i].Test.TotalNoOfHardQuestions,
                        reportList[i].Test.TotalNoOfQuestions,
                        Result = reportList[i].Result.Name
                    };
                }

                return Ok(reportResponseList);
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

        // student route
        [HttpGet]
        [Route("GetReportsByOrgName")]
        public IHttpActionResult GetReportsByOrganizationName(string organizationName)
        {
            try
            {
                if(organizationName == null)
                {
                    throw new NullReferenceException("Organization Name cannot be null");
                }

                // extracting student id
                ClaimsPrincipal student = HttpContext.Current.User as ClaimsPrincipal;
                string studentId = student.Claims.ElementAt(1).Value;

                // fetching all the reports of tests that student gave for a particular organization
                List<Report> reportList = _reportRepository.GetAll(r => r.Test.Admin.OrganizationName == organizationName && r.StudentId == studentId, "Student,Test,Result").ToList();
                if(reportList == null)
                {
                    throw new NullEntityException("Reports Not Found");
                }

                // creating response
                Object[] reportResponseList = new object[reportList.Count];
                for(int i=0;i<reportList.Count;i++)
                {
                    reportResponseList[i] = new
                    {
                        reportList[i].Student.Username,
                        TestName = reportList[i].Test.Name,
                        reportList[i].Test.Admin.OrganizationName,
                        TestDuration = reportList[i].Test.Duration,
                        CorrectAttemptsInEasyQuestions = reportList[i].CorrectAttempsInEasyQuestions,
                        reportList[i].TotalAttemptsInEasyQuestions,
                        reportList[i].Test.TotalNoOfEasyQuestions,
                        reportList[i].CorrectAttemptsInMediumQuestions,
                        reportList[i].TotalAttemptsInMediumQuestions,
                        reportList[i].Test.TotalNoOfMediumQuestions,
                        reportList[i].CorrectAttemptsInHardQuestions,
                        reportList[i].TotalAttemptsInHardQuestions,
                        reportList[i].Test.TotalNoOfHardQuestions,
                        reportList[i].Test.TotalNoOfQuestions,
                        Result = reportList[i].Result.Name
                    };
                }

                return Ok(reportResponseList);
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

        // route for both admin and student
        [HttpGet]
        [Route("GetReportById")]
        public IHttpActionResult GetReportById(string reportId)
        {
            try
            {
                if(reportId == null)
                {
                    throw new NullReferenceException("ReportId cannot be null");
                }

                // fetching report for the given reportId
                Report report = _reportRepository.Get(r => r.Id == reportId, "Student,Test,Result");
                if(report == null)
                {
                    throw new NullEntityException("Report Not Found");
                }

                Object reportResponse = new
                {
                    Student = new
                    {
                        report.Student.Username,
                        report.Student.Email,
                        report.Student.PhoneNumber,
                        report.Student.DOB,
                        report.Student.GraduationYear,
                        report.Student.City,
                        report.Student.UniversityName,
                        report.Student.DegreeMajor
                    },
                    Test = new
                    {
                        report.Test.Name,
                        report.Test.Description,
                        CorrectAttemptsInEasyQuestions = report.CorrectAttempsInEasyQuestions,
                        report.TotalAttemptsInEasyQuestions,
                        report.Test.TotalNoOfEasyQuestions,
                        report.CorrectAttemptsInMediumQuestions,
                        report.TotalAttemptsInMediumQuestions,
                        report.Test.TotalNoOfMediumQuestions,
                        report.CorrectAttemptsInHardQuestions,
                        report.TotalAttemptsInHardQuestions,
                        report.Test.TotalNoOfHardQuestions,
                        report.Test.TotalNoOfQuestions,
                        report.Test.Duration,
                        report.Test.Admin.OrganizationName,
                        Result = report.Result.Name
                    }
                };

                return Ok(reportResponse);
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

        // admin route
        [HttpDelete]
        [Route("Remove")]
        public IHttpActionResult RemoveReport(string reportId)
        {
            try
            {
                if(reportId == null)
                {
                    throw new NullReferenceException("Report Id cannot be null");
                }

                // extracting adminId
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);

                // checking if the report exist or not
                Report report = _reportRepository.Get(r => r.Id == reportId, "Test");
                if(report == null)
                {
                    throw new NullEntityException("Report Not Found");
                }

                // checking if the admin is authorized to delete the report
                if(report.Test.AdminId != adminId)
                {
                    throw new UnauthorizedAccessException();
                }

                // deleting the report
                if(_reportRepository.Remove(report) && _reportRepository.Save())
                {
                    return Ok();
                }
                else
                {
                    throw new OperationFailedException("Report Cannot be deleted");
                }
            }
            catch(OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
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

        // admin route
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateReport(string reportId, [FromBody] ReportAddOrUpdateModel reportUpdateModel)
        {
            try
            {
                if(reportId == null || reportId.Length == 0)
                {
                    throw new NullReferenceException("Report Id cannot be empty");
                }

                // extracting admin Id
                ClaimsPrincipal admin = HttpContext.Current.User as ClaimsPrincipal;
                int adminId = int.Parse(admin.Claims.ElementAt(1).Value);

                // checking if the report exist or not
                Report reportExist = _reportRepository.Get(r => r.Id == reportId, "Test");
                if(reportExist == null)
                {
                    throw new NullEntityException("Report Not Found");
                }

                // checking if the admin is authorized to udpate the report
                if(reportExist.Test.AdminId != adminId)
                {
                    throw new UnauthorizedAccessException();
                }

                // updating report
                Report updatedReport = _reportRepository.Update(reportId, reportUpdateModel);
                if(updatedReport != null && _reportRepository.Save())
                {
                    return Ok();
                }
                else
                {
                    throw new OperationFailedException("Report Cannot be Updated");
                }
            }
            catch(OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
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

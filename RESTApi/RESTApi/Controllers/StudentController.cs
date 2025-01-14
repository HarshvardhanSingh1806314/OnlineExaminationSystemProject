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
using System.Web.Http;
using static RESTApi.Models.CustomModels;

namespace RESTApi.Controllers
{
    [RoutePrefix("api/Student")]
    public class StudentController : ApiController
    {
        private readonly ApplicationContext _db;

        private readonly IStudentRepository _studentRepository;

        public StudentController()
        {
            _db = new ApplicationContext();
            _studentRepository = new StudentRepository(_db);
        }

        [HttpPut]
        [Route("ResetPassword")]
        public IHttpActionResult ResetPassword(ResetStudentPassword resetStudentPassword)
        {
            try
            {
                if(resetStudentPassword.Email == null || resetStudentPassword.NewPassword == null || resetStudentPassword.ConfirmPassword == null)
                {
                    throw new NullReferenceException("Reset password fields cannot be null");
                }

                if(resetStudentPassword.NewPassword != resetStudentPassword.ConfirmPassword)
                {
                    throw new ConflictException("New password and Confirm Password do not match");
                }

                // fetching the student with given email
                Student studentExist = _studentRepository.Get(s => s.Email == resetStudentPassword.Email);
                if(studentExist == null)
                {
                    throw new NullEntityException("Not Found");
                }

                // hashing the new password
                string hashedPassword = PasswordManager.HashPassword(resetStudentPassword.NewPassword);
                StudentAddOrUpdateModel studentUpdateModel = new StudentAddOrUpdateModel
                {
                    Password = hashedPassword
                };
                if(_studentRepository.Update(studentExist.Id, studentUpdateModel) != null && _studentRepository.Save())
                {
                    return Ok();
                }
                else
                {
                    throw new OperationFailedException("Password Updation Failed");
                }
            }
            catch(OperationFailedException ex)
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

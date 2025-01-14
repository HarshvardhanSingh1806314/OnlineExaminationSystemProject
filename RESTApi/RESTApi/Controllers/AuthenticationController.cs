using RESTApi.CustomExceptions;
using RESTApi.DataAccess;
using RESTApi.DataAccess.Repositories;
using RESTApi.Models;
using RESTApi.Utility;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using static RESTApi.Models.CustomModels;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace RESTApi.Controllers
{
    [RoutePrefix("api/Auth")]
    public class AuthenticationController : ApiController
    {
        private readonly ApplicationContext _db;

        private readonly StudentRepository _studentRepository;

        private readonly AdminRepository _adminRepository;

        public AuthenticationController()
        {
            _db = new ApplicationContext();
            _studentRepository = new StudentRepository(_db);
            _adminRepository = new AdminRepository(_db);
            StaticDetails.JWT_TOKEN_SECRET_KEY = IdGenerator.GenerateSecretKey();
        }

        [HttpPost]
        [Route("Student/Register")]
        public IHttpActionResult RegisterStudent([FromBody] StudentRegisterModel studentRegisterModel)
        {
            try
            {
                // checking if the register model properties are null or not
                if (studentRegisterModel.Username == null ||
                   studentRegisterModel.Email == null ||
                   studentRegisterModel.Password.Length < 8 ||
                   studentRegisterModel.PhoneNumber == null ||
                   studentRegisterModel.DOB == null ||
                   studentRegisterModel.GraduationYear <= 0 ||
                   studentRegisterModel.UniversityName == null ||
                   studentRegisterModel.DegreeMajor == null)
                {
                    throw new NullEntityException("Student Registration Information Incomplete.");
                }

                // checking if the student already exist
                Student studentExist = _studentRepository.Get(s => s.Email == studentRegisterModel.Email);
                if(studentExist != null)
                {
                    throw new UserAlreadyExistException($"Student with email: {studentExist.Email} already exist");
                }

                // generating the id for new student
                string studentId = IdGenerator.GenerateIdForStudent(studentRegisterModel.Email, studentRegisterModel.PhoneNumber);

                // hashing the password for student
                string hashedPassword = PasswordManager.HashPassword(studentRegisterModel.Password);

                // creating a new Student entity
                Student student = new Student
                {
                    Id = studentId,
                    Username = studentRegisterModel.Username,
                    Email = studentRegisterModel.Email,
                    Password = hashedPassword,
                    PhoneNumber = studentRegisterModel.PhoneNumber,
                    DOB = studentRegisterModel.DOB,
                    GraduationYear = studentRegisterModel.GraduationYear,
                    City = studentRegisterModel.City,
                    UniversityName = studentRegisterModel.UniversityName,
                    DegreeMajor = studentRegisterModel.DegreeMajor
                };

                // adding new student to database
                Student newAddedStudent = _studentRepository.Add(student);
                if(newAddedStudent != null && _studentRepository.Save())
                {
                    string roleId = _db.Roles.Where(r => r.Name == StaticDetails.ROLE_STUDENT).FirstOrDefault().RoleId;
                    _db.UserRoles.Add(new UserRole { UserId = newAddedStudent.Id, RoleId = roleId });
                    _db.SaveChanges();
                    return Created("Student", new { 
                        newAddedStudent.Username
                    });
                }
                else
                {
                    throw new OperationFailedException($"Failed to register student with email: {studentRegisterModel.Email}");
                }
            }
            catch(OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UserAlreadyExistException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullEntityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Admin/Register")]
        public IHttpActionResult RegisterAdmin([FromBody] AdminRegisterModel adminRegisterModel)
        {
            try
            {
                // checking if admin entity is null
                if(adminRegisterModel.Username == null ||
                   adminRegisterModel.EmployeeEmail == null ||
                   adminRegisterModel.OrganizationName == null ||
                   adminRegisterModel.EmployeeId <= 0)
                {
                    throw new NullEntityException("Admin Registration Information Incomplete.");
                }

                // checking if admin already exist
                Admin adminExist = _adminRepository.Get(ad => ad.EmployeeEmail == adminRegisterModel.EmployeeEmail);
                if(adminExist != null)
                {
                    throw new UserAlreadyExistException($"Admin with email: {adminRegisterModel.EmployeeEmail} already exist");
                }

                // generating admin id
                List<int> adminsList = _db.Admins.Select(ad => ad.AdminId).ToList();
                int adminId = IdGenerator.GenerateIdForAdmin(adminsList);

                // generating password for admin
                string adminPassword = PasswordManager.GenerateAdminPassword();
                string hashedPassword = PasswordManager.HashPassword(adminPassword);

                // creating new Admin entity
                Admin admin = new Admin
                {
                    AdminId = adminId,
                    Password = hashedPassword,
                    Username = adminRegisterModel.Username,
                    EmployeeEmail = adminRegisterModel.EmployeeEmail,
                    OrganizationName = adminRegisterModel.OrganizationName,
                    EmployeeId = adminRegisterModel.EmployeeId
                };

                // adding admin to database
                Admin newAddedAdmin = _adminRepository.Add(admin);
                if (newAddedAdmin != null && _adminRepository.Save())
                {
                    string roleId = _db.Roles.Where(r => r.Name == StaticDetails.ROLE_ADMIN).FirstOrDefault().RoleId;
                    _db.UserRoles.Add(new UserRole { UserId = newAddedAdmin.AdminId.ToString(), RoleId = roleId });
                    _db.SaveChanges();
                    object response = new
                    {
                        newAddedAdmin.AdminId,
                        Password = adminPassword
                    };
                    return Created("Admin", response);
                }
                else
                {
                    throw new OperationFailedException("Admin Registration Failed");
                }
            }
            catch (NullEntityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserAlreadyExistException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(OperationFailedException ex)
            {
                return BadRequest (ex.Message);
            }
            catch(Exception ex)
            {
                return Ok(ex.InnerException);
            }
        }

        private string GenerateJwtToken(string role, string userId)
        {
            // creating the security key that will be used to sign the jwt token
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticDetails.JWT_TOKEN_SECRET_KEY));

            // hashing the security key to generate signing credentials for jwt token
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // creating claims that will be included in the token
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Role, role),
                new Claim("UserId", userId)
            };

            // creating token
            JwtSecurityToken authenticationToken = new JwtSecurityToken(
                issuer: "OnlineExaminationSystemBackend",
                audience: "OnlineExaminationSystemFrontend",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(authenticationToken);
        }

        [HttpPost]
        [Route("Student/Login")]
        public IHttpActionResult LoginStudent([FromBody] StudentLoginModel studentLoginModel)
        {
            try
            {
                // checking if all the fields are populated correctly
                if (studentLoginModel.Email == null || studentLoginModel.Email.Length == 0
                    || studentLoginModel.Password == null || studentLoginModel.Password.Length < 8
                )
                {
                    throw new InvalidCredentialsException("Invalid Student Login Credentials");
                }

                // checking if the user exist or not
                Student studentExist = _studentRepository.Get(st => st.Email == studentLoginModel.Email);
                if(studentExist == null)
                {
                    throw new UserNotFoundException($"Student with email: {studentLoginModel.Email} does not exist");
                }

                // checking if the entered password is correct or not
                bool isPasswordCorrect = PasswordManager.VerifyPassword(studentExist.Password, studentLoginModel.Password);
                if(!isPasswordCorrect)
                {
                    throw new InvalidCredentialsException("Invalid Email or Password");
                }

                // creating jwt token
                UserRole userRole = _db.UserRoles.Include("Role").FirstOrDefault(ur => ur.UserId == studentExist.Id);
                return Ok(new { studentExist.Username, AccessToken = GenerateJwtToken(userRole.Role.Name, studentExist.Id) });
            }
            catch(InvalidCredentialsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UserNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("Admin/Login")]
        public IHttpActionResult AdminLogin([FromBody] AdminLoginModel adminLoginModel)
        {
            try
            {
                // checking if admin credentials are null or not
                if(adminLoginModel.AdminId < 100000 || adminLoginModel.Password == null || adminLoginModel.Password.Length == 0)
                {
                    throw new InvalidCredentialsException("Invalid Admin Login Credentials");
                }

                // checking if admin exist or not
                Admin adminExist = _adminRepository.Get(ad => ad.AdminId == adminLoginModel.AdminId);
                if(adminExist == null)
                {
                    throw new UserNotFoundException($"Admin with Id: {adminLoginModel.AdminId} Not Found");
                }

                // cheking if admin password is correct or not
                bool isPasswordCorrect = PasswordManager.VerifyPassword(adminExist.Password, adminLoginModel.Password);
                if(!isPasswordCorrect)
                {
                    throw new InvalidCredentialsException("Invalid Email or Password");
                }

                // creating jwt token
                UserRole userRole = _db.UserRoles.Include("Role").FirstOrDefault(ur => ur.UserId == adminExist.AdminId.ToString());
                return Ok(new { adminExist.Username, AccessToken = GenerateJwtToken(userRole.Role.Name, adminExist.AdminId.ToString()) });
            }
            catch(InvalidCredentialsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UserNotFoundException)
            {
                return NotFound();
            }
        }
    }
}

using RESTApi.DataAccess.Repositories.IRepository;
using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly ApplicationContext _db;

        public StudentRepository(ApplicationContext db) : base(db)
        {
            _db = db;
        }

        public Student Update(string StudentId, StudentAddOrUpdateModel studentUpdateModel)
        {
            Student studentExist = _db.Students.Find(StudentId);

            studentExist.Username = studentUpdateModel.Username != null && studentUpdateModel.Username.Length > 0 ? 
                                    studentUpdateModel.Username : studentExist.Username;

            studentExist.Email = studentUpdateModel.Email != null && studentUpdateModel.Email.Length > 0 ?
                                 studentUpdateModel.Email : studentExist.Email;

            studentExist.Password = studentUpdateModel.Password != null && studentUpdateModel.Password.Length > 0 ?
                                    studentUpdateModel.Password : studentExist.Password;

            studentExist.PhoneNumber = studentUpdateModel.PhoneNumber != null && studentUpdateModel.PhoneNumber.Length > 0 ?
                                       studentUpdateModel.PhoneNumber : studentExist.PhoneNumber;

            studentExist.DOB = studentUpdateModel.DOB != null && studentUpdateModel.DOB.ToString().Length > 0 ?
                               studentUpdateModel.DOB : studentExist.DOB;

            studentExist.GraduationYear = studentUpdateModel.GraduationYear > 1970 ? studentUpdateModel.GraduationYear : studentExist.GraduationYear;

            studentExist.City = studentUpdateModel.City;

            studentExist.UniversityName = studentUpdateModel.UniversityName != null && studentUpdateModel.UniversityName.Length > 0 ?
                                          studentUpdateModel.UniversityName : studentExist.UniversityName;

            studentExist.DegreeMajor = studentUpdateModel.DegreeMajor != null && studentUpdateModel.DegreeMajor.Length > 0 ?
                                       studentUpdateModel.DegreeMajor : studentExist.DegreeMajor;

            return studentExist;
        }
    }
}
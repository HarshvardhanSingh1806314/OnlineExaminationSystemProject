using System;

namespace RESTApi.Models
{
    public class CustomModels
    {
        public struct TestUpdateModel
        {
            public string Name { get; set; }

            public string Description { get; set; }
        }

        public struct QuestionUpdateModel
        {
            public string Description { get; set; }

            public string Option1 { get; set; }

            public string Option2 { get; set; }

            public string Option3 { get; set; }

            public string Option4 { get; set; }

            public string Answer { get; set; }
        }

        public struct StudentUpdateModel
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string PhoneNumber { get; set; }

            public DateTime DOB { get; set; }

            public int GraduationYear { get; set; }

            public string City { get; set; }

            public string UniversityName { get; set; }

            public string DegreeMajor { get; set; }
        }

        public struct AdminUpdateModel
        {
            public int Id { get; set; }

            public string Username { get; set; }

            public string EmployeeEmail { get; set; }

            public string Password { get; set; }

            public string OrganizationName { get; set; }

            public int EmployeeId { get; set; }
        }

        public struct ReportUpdateModel
        {
            public string StudentId { get; set; }

            public string TestId { get; set; }

            public int TotalAttemptsInEasyQuestions { get; set; }

            public int CorrectAttemptsInEasyQuestions { get; set; }

            public int TotalAttemptsInMediumQuestions { get; set; }

            public int CorrectAttemptsInMediumQuestions { get; set; }

            public int TotalAttemptsInHardQuestions { get; set; }

            public int CorrectAttemptsInHardQuestions { get; set; }
        }
    }
}
using System;

namespace RESTApi.Models
{
    /* These models will be used when the request to update any of the respective
     * database models come.
     * 
     * The login models will be used in login Post methods for both Student and Admin
     * and these login models will be populated with the values provided in the request body
     * 
     * These models will be populated with the respective fields in the request body
     */
    public class CustomModels
    {
        public struct TestAddOrUpdateModel
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public int Duration { get; set; }
        }

        public struct QuestionAddOrUpdateModel
        {
            public string Description { get; set; }

            public string Option1 { get; set; }

            public string Option2 { get; set; }

            public string Option3 { get; set; }

            public string Option4 { get; set; }

            public string Answer { get; set; }

            public string DifficultyLevel { get; set; }
        }

        public struct StudentAddOrUpdateModel
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string PhoneNumber { get; set; }

            public string DOB { get; set; }

            public int GraduationYear { get; set; }

            public string City { get; set; }

            public string UniversityName { get; set; }

            public string DegreeMajor { get; set; }
        }

        public struct AdminAddOrUpdateModel
        {
            public int Id { get; set; }

            public string Username { get; set; }

            public string EmployeeEmail { get; set; }

            public string Password { get; set; }

            public string OrganizationName { get; set; }

            public int EmployeeId { get; set; }
        }

        public struct ReportAddOrUpdateModel
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

        public struct StudentLoginModel
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public struct StudentRegisterModel
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string PhoneNumber { get; set; }

            public string DOB { get; set; }

            public int GraduationYear { get; set; }

            public string City { get; set; }

            public string UniversityName { get; set; }

            public string DegreeMajor { get; set; }
        }

        public struct AdminLoginModel
        {
            public int AdminId { get; set; }

            public string Password { get; set; }
        }

        public struct AdminRegisterModel
        {
            public string Username { get; set; }

            public string EmployeeEmail { get; set; }

            public string OrganizationName { get; set; }

            public int EmployeeId { get; set; }
        }
    }
}
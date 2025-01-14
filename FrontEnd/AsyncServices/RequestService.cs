using FrontEnd.Models;
using FrontEnd.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static FrontEnd.Models.ResponseModels;
using static FrontEnd.Utility.StaticDetails;

namespace FrontEnd.AsyncServices
{
    public static class RequestService
    {
        public static HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44378")
        };

        private static HttpContent CreateRequestContent(object requestData)
        {
            string data = JsonConvert.SerializeObject(requestData);
            return new StringContent(data, Encoding.UTF8, "application/json");
        }

        private static string EncodeUrl(string input)
        {
            for(int i=0;i<input.Length;i++)
            {
                switch(input[i])
                {
                    case CHARACTER_SPACE:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_SPACE + input.Substring(i + 1);
                        break;
                    case CHARACTER_PLUS:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_PLUS + input.Substring(i + 1);
                        break;
                    case CHARACTER_AMPERSAND:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_AMPERSAND + input.Substring(i + 1);
                        break;
                    case CHARACTER_EQUAL:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_EQUAL + input.Substring(i + 1);
                        break;
                    case CHARACTER_HASH:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_HASH + input.Substring(i + 1);
                        break;
                    case CHARACTER_QUESTION:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_QUESTION + input.Substring(i + 1);
                        break;
                    case CHARACTER_FORWARD_SLASH:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_FORWARD_SLASH + input.Substring(i + 1);
                        break;
                    case CHARACTER_COLON:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_COLON + input.Substring(i + 1);
                        break;
                    case CHARACTER_SEMI_COLON:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_SEMI_COLON + input.Substring(i + 1);
                        break;
                    case CHARACTER_AT:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_AT + input.Substring(i + 1);
                        break;
                    case CHARACTER_OPEN_SQUARE_BRACKET:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_OPEN_SQUARE_BRACKET + input.Substring(i + 1);
                        break;
                    case CHARACTER_CLOSE_SQUARE_BRACKET:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_CLOSE_SQUARE_BRACKET + input.Substring(i + 1);
                        break;
                    case CHARACTER_OPEN_CURLY_BRACKET:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_OPEN_CURLY_BRACKET + input.Substring(i + 1);
                        break;
                    case CHARACTER_CLOSE_CURLY_BRACKET:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_CLOSE_CURLY_BRACKET + input.Substring(i + 1);
                        break;
                    case CHARACTER_PERCENT:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_PERCENT + input.Substring(i + 1);
                        break;
                    case CHARACTER_CARET:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_CARET + input.Substring(i + 1);
                        break;
                    case CHARACTER_LESS_THAN:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_LESS_THAN + input.Substring(i + 1);
                        break;
                    case CHARACTER_GREATER_THAN:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_GREATER_THAN + input.Substring(i + 1);
                        break;
                    case CHARACTER_DOUBLE_QUOTE:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_DOUBLE_QUOTE + input.Substring(i + 1);
                        break;
                    case CHARACTER_SINGLE_QUOTE:
                        input = input.Substring(0, i) + ENCODED_VALUE_FOR_SINGLE_QUOTE + input.Substring(i + 1);
                        break;
                }

            }
            
            return input;
        }

        private static async Task<string> SendRequest(string apiEndPoint, string requestType, HttpContent requestData = null)
        {
            HttpResponseMessage httpResponse = null;
            switch (requestType)
            {
                case REQUEST_TYPE_GET:
                    httpResponse = await httpClient.GetAsync(apiEndPoint);
                    break;
                case REQUEST_TYPE_POST:
                    httpResponse = await httpClient.PostAsync(apiEndPoint, requestData);
                    break;
                case REQUEST_TYPE_PUT:
                    httpResponse = await httpClient.PutAsync(apiEndPoint, requestData);
                    break;
                case REQUEST_TYPE_DELETE:
                    httpResponse = await httpClient.DeleteAsync(apiEndPoint);
                    break;
            }
            if(httpResponse != null && httpResponse.IsSuccessStatusCode)
            {
                string response = await httpResponse.Content.ReadAsStringAsync();
                return response != null && response.Length > 0 ? response : StaticDetails.RESPONSE_OK;
            }

            return null;
        }

        public static async Task<LoginModel> StudentLoginServive(StudentLogin studentLoginCredentials)
        {
            HttpContent postLoginRequestData = CreateRequestContent(studentLoginCredentials);
            string response = await SendRequest("/api/Auth/Student/Login", REQUEST_TYPE_POST, postLoginRequestData);
            LoginModel loginResponse = JsonConvert.DeserializeObject<LoginModel>(response);
            return loginResponse;
        }

        public static async Task<object> StudentRegisterService(Student studentRegistrationData)
        {
            HttpContent postRegisterRequestData = CreateRequestContent(studentRegistrationData);
            string response = await SendRequest("/api/Auth/Student/Register", REQUEST_TYPE_POST, postRegisterRequestData);
            return JsonConvert.DeserializeObject<object>(response);
        }

        public static async Task<LoginModel> AdminLoginService(Admin adminLoginCredentials)
        {
            HttpContent postLoginRequestData = CreateRequestContent(adminLoginCredentials);
            string response = await SendRequest("/api/Auth/Admin/Login", REQUEST_TYPE_POST, postLoginRequestData);
            LoginModel loginResponse = JsonConvert.DeserializeObject<LoginModel>(response);
            return loginResponse;
        }

        public static async Task<Test> CreateNewTest(AddTest test, string accessToken)
        {
            HttpContent postAddTestRequest = CreateRequestContent(test);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string response = await SendRequest("/api/Test/Add", REQUEST_TYPE_POST, postAddTestRequest);
            return JsonConvert.DeserializeObject<Test>(response);
        }

        public static async Task<List<Test>> GetAllTests(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string response = await SendRequest("/api/Test/GetAllTests", REQUEST_TYPE_GET);
            return JsonConvert.DeserializeObject<List<Test>>(response);
        }

        public static async Task<TestDropDown> GetAllTestAndTestIdList(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string response = await SendRequest("/api/Test/GetAllTestAndTestIdList", REQUEST_TYPE_GET);
            List<TestNameAndId> testList = JsonConvert.DeserializeObject<List<TestNameAndId>>(response);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach(var item in testList)
            {
                selectListItems.Add(new SelectListItem { Text = item.Name, Value = item.TestId });
            }
            TestDropDown testDropDown = new TestDropDown {
                Items = selectListItems
            };
            return testDropDown;
        }

        public static async Task<List<Questions>> GetQuestionsByTestId(string testId, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            testId = EncodeUrl(testId);
            string response = await SendRequest($"/api/Question/GetQuestionsByTestId?testId={testId}", REQUEST_TYPE_GET);
            if(response == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<List<Questions>>(response);
        }

        public static async Task<Questions> CreateQuestion(string testId, CreateQuestion question, string accessToken)
        {
            HttpContent postAddQuestionRequestData = CreateRequestContent(question);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            testId = EncodeUrl(testId);
            string response = await SendRequest($"/api/Question/Add?testId={testId}", REQUEST_TYPE_POST, postAddQuestionRequestData);
            if(response != null)
                return JsonConvert.DeserializeObject<Questions>(response);

            return null;
        }

        public static async Task<CreateQuestion> GetQuestionById(string questionId, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            questionId = EncodeUrl(questionId);
            string response = await SendRequest($"api/Question/GetQuestionById?questionId={questionId}", REQUEST_TYPE_GET);
            if(response != null)
            {
                return JsonConvert.DeserializeObject<CreateQuestion>(response);
            }

            return null;
        }

        public static async Task<Questions> UpdateQuestion(string questionId, string testId, CreateQuestion question, string accessToken)
        {
            HttpContent putUpdateQuestionRequestData = CreateRequestContent(question);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            questionId = EncodeUrl(questionId);
            testId = EncodeUrl(testId);
            string response = await SendRequest($"/api/Question/Update?testId={testId}&questionId={questionId}", REQUEST_TYPE_PUT, putUpdateQuestionRequestData);
            if(response != null)
            {
                return JsonConvert.DeserializeObject<Questions>(response);
            }

            return null;
        }

        public static async Task<bool> DeleteQuestion(string questionId, string testId, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            questionId = EncodeUrl(questionId);
            testId = EncodeUrl(testId);
            string response = await SendRequest($"/api/Question/Remove?testId={testId}&questionId={questionId}", REQUEST_TYPE_DELETE);
            return response == RESPONSE_OK;
        }

        public static async Task<List<StudentReports>> GetReportsByTestId(string testId, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            testId = EncodeUrl(testId);
            string response = await SendRequest($"/api/Report/GetReportsByTestId?testId={testId}", REQUEST_TYPE_GET);
            if (response != null)
                return JsonConvert.DeserializeObject<List<StudentReports>>(response);

            return null;
        }

        public static async Task<List<UserTest>> GetTests(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string response = await SendRequest("/api/Test/GetTests", REQUEST_TYPE_GET);
            if(response != null)
            {
                return JsonConvert.DeserializeObject<List<UserTest>>(response);
            }

            return null;
        }

        public static async Task<List<Questions>> GetQuestionsByTestIdAndDifficultyLevel(string testId, string difficultyLevel, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            testId = EncodeUrl(testId);
            string response = await SendRequest($"/api/Question/GetQuestionsByTestIdAndDifficultyLevel?testId={testId}&difficultyLevel={difficultyLevel}", REQUEST_TYPE_GET);
            if(response != null)
            {
                return JsonConvert.DeserializeObject<List<Questions>>(response);
            }

            return null;
        }

        public static async Task<Result> SubmitTest(string testId, string difficultyLevel, SubmitQuestionResponse questionResponses, string accessToken)
        {
            HttpContent postSubmitTestRequestData = CreateRequestContent(questionResponses);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            testId = EncodeUrl(testId);
            string response = await SendRequest($"/api/Test/SubmitTest?testId={testId}&difficultyLevel={difficultyLevel}", REQUEST_TYPE_POST, postSubmitTestRequestData);
            if(response != null)
            {
                return JsonConvert.DeserializeObject<Result>(response);
            }

            return null;
        }

        public static async Task<List<StudentReport>> GetAllStudentReports(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string response = await SendRequest($"/api/Report/GetAllStudentReports", REQUEST_TYPE_GET);
            if (response != null)
                return JsonConvert.DeserializeObject<List<StudentReport>>(response);

            return null;
        }

        public static async Task<bool> ResetStudentPassword(Resetpassword resetpassword)
        {
            HttpContent postResetPasswordRequestData = CreateRequestContent(resetpassword);
            string response = await SendRequest("/api/Student/ResetPassword", REQUEST_TYPE_PUT, postResetPasswordRequestData);
            return response == RESPONSE_OK;
        }
    }
}
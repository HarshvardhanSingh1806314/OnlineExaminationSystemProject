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
using static FrontEnd.Models.ResponseModels;

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

        private static async Task<string> SendRequest(string apiEndPoint, string requestType, HttpContent requestData = null)
        {
            HttpResponseMessage httpResponse = null;
            switch (requestType)
            {
                case StaticDetails.REQUEST_TYPE_GET:
                    httpResponse = await httpClient.GetAsync(apiEndPoint);
                    break;
                case StaticDetails.REQUEST_TYPE_POST:
                    httpResponse = await httpClient.PostAsync(apiEndPoint, requestData);
                    break;
                case StaticDetails.REQUEST_TYPE_PUT:
                    httpResponse = await httpClient.PutAsync(apiEndPoint, requestData);
                    break;
                case StaticDetails.REQUEST_TYPE_DELETE:
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

        public static async Task<string> StudentLoginServive(StudentLogin studentLoginCredentials)
        {
            HttpContent postLoginRequestData = CreateRequestContent(studentLoginCredentials);
            string response = await SendRequest("/api/Auth/Student/Login", StaticDetails.REQUEST_TYPE_POST, postLoginRequestData);
            LoginModel loginResponse = JsonConvert.DeserializeObject<LoginModel>(response);
            return loginResponse.AccessToken;
        }

        public static async Task<bool> StudentRegisterService(Student studentRegistrationData)
        {
            HttpContent postRegisterRequestData = CreateRequestContent(studentRegistrationData);
            string response = await SendRequest("/api/Auth/Student/Register", StaticDetails.REQUEST_TYPE_POST, postRegisterRequestData);
            return response == StaticDetails.RESPONSE_OK;
        }

        public static async Task<string> AdminLoginService(Admin adminLoginCredentials)
        {
            HttpContent postLoginRequestData = CreateRequestContent(adminLoginCredentials);
            string response = await SendRequest("/api/Auth/Admin/Login", StaticDetails.REQUEST_TYPE_POST, postLoginRequestData);
            LoginModel loginResponse = JsonConvert.DeserializeObject<LoginModel>(response);
            return loginResponse.AccessToken;
        }

        public static async Task<Test> CreateNewTest(AddTest test, string accessToken)
        {
            HttpContent postAddTestRequest = CreateRequestContent(test);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string response = await SendRequest("/api/Test/Add", StaticDetails.REQUEST_TYPE_POST, postAddTestRequest);
            return JsonConvert.DeserializeObject<Test>(response);
        }

        public static async Task<List<Test>> GetAllTests(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string response = await SendRequest("/api/Test/GetAllTests", StaticDetails.REQUEST_TYPE_GET);
            return JsonConvert.DeserializeObject<List<Test>>(response);
        }
    }
}
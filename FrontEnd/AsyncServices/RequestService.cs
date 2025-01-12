using FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        private static async Task<string> SendRequest(string apiEndPoint, HttpContent requestData)
        {
            HttpResponseMessage httpResponse = await httpClient.PostAsync(apiEndPoint, requestData);
            if(httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadAsStringAsync();
            }

            return null;
        }

        public static async Task<string> StudentLoginServive(StudentLogin studentLoginCredentials)
        {
            HttpContent postLoginRequestData = CreateRequestContent(studentLoginCredentials);
            string response = await SendRequest("/api/Auth/Student/Login", postLoginRequestData);
            LoginModel loginResponse = JsonConvert.DeserializeObject<LoginModel>(response);
            return loginResponse.AccessToken;
        }

        public static async Task<bool> StudentRegisterService(Student studentRegistrationData)
        {
            HttpContent postRegisterRequestData = CreateRequestContent(studentRegistrationData);
            HttpResponseMessage httpResponse = await httpClient.PostAsync("/api/Auth/Student/Register", postRegisterRequestData);
            return httpResponse.IsSuccessStatusCode;
        }

        public static async Task<string> AdminLoginService(Admin adminLoginCredentials)
        {
            HttpContent postLoginRequestData = CreateRequestContent(adminLoginCredentials);
            string response = await SendRequest("/api/Auth/Admin/Login", postLoginRequestData);
            LoginModel loginResponse = JsonConvert.DeserializeObject<LoginModel>(response);
            return loginResponse.AccessToken;
        }
    }
}
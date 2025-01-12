using Microsoft.IdentityModel.Tokens;
using RESTApi.Utility;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Web;

namespace RESTApi.Middlewares
{
    public class JwtAuthenticationHandler : DelegatingHandler
    {
        // overriding the SendAsync method to intercept the HTTP request
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Skipping token validation for selected routes
            if (IsAuthRoute(request))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            // checking if the Authorization header contains the token
            if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Bearer")
            {
                string authToken = request.Headers.Authorization.Parameter;
                try
                {
                    // validating the token
                    ClaimsPrincipal principal = ValidateToken(authToken);

                    // checking if the user is authorized to access the endpoint based on there role
                    if(!IsUserAuthorizedForRequestedRoute(request, principal.FindFirst(ClaimTypes.Role).Value))
                    {
                        return request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to access this path");
                    }

                    // If token is valid then claims are added to HttpContext for further use in the api
                    HttpContext.Current.User = principal;
                }
                catch (Exception ex)
                {
                    return request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "Invalid Token" + ex.Message);
                }
            }
            else
            {
                return request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "Authorization Token is missing");
            }

            // proceed with the request to the controller
            return await base.SendAsync(request, cancellationToken);
        }

        // method to check if the user id authorized to access the route based on their role
        private bool IsUserAuthorizedForRequestedRoute(HttpRequestMessage requestMessage, string role)
        {
            string path = requestMessage.RequestUri.AbsolutePath.ToLower();
            string route = "";
            switch(role)
            {
                case StaticDetails.ROLE_ADMIN:
                    route = StaticDetails.ADMIN_ROUTES.FirstOrDefault(ar => ar.Equals(path));
                    break;
                case StaticDetails.ROLE_STUDENT:
                    route = StaticDetails.STUDENT_ROUTES.FirstOrDefault(ar => ar.Equals(path));
                    break;
            }

            if (route != null && route.Length != 0)
            {
                return true;
            }
            route = StaticDetails.COMMON_ROUTES.FirstOrDefault(cr => cr.Equals(path));

            return route != null && route.Length != 0;
        }

        // Method to check if the request contains the route which need to exclude token validation
        private bool IsAuthRoute(HttpRequestMessage request)
        {
            string path = request.RequestUri.AbsolutePath.ToLower();
            return path.Contains(StaticDetails.STUDENT_LOGIN_PATH) || path.Contains(StaticDetails.STUDENT_REGISTER_PATH)
                || path.Contains(StaticDetails.ADMIN_LOGIN_PATH) || path.Contains(StaticDetails.ADMIN_REGISTER_PATH);
        }

        // validating JWT token
        private ClaimsPrincipal ValidateToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] secretKey = Encoding.UTF8.GetBytes(StaticDetails.JWT_TOKEN_SECRET_KEY);

            // setting up validation parameters
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = StaticDetails.ISSUER,
                ValidAudience = StaticDetails.AUDIENCE,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ClockSkew = TimeSpan.Zero
            };

            // validating and decoding the token
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
    }
}
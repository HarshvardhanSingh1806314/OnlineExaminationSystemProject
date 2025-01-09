using Microsoft.IdentityModel.Tokens;
using RESTApi.Utility;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RESTApi.Middlewares
{
    public class JwtAuthenticationHandler : DelegatingHandler
    {
        // Issuer of the token
        private const string Issuer = "OnlineExaminationSystemBackend";

        // Audience of the token
        private const string Audience = "OnlineExaminationSystemFrontend";

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

        // Method to check if the request contains the route which need to exclude token validation
        private bool IsAuthRoute(HttpRequestMessage request)
        {
            string path = request.RequestUri.AbsolutePath.ToLower();
            return path.Contains("/api/auth/student/login") || path.Contains("/api/auth/student/register")
                || path.Contains("/api/auth/admin/login") || path.Contains("api/auth/admin/register");
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
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ClockSkew = TimeSpan.Zero
            };

            // validating and decoding the token
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
    }
}
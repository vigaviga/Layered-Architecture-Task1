using System.IdentityModel.Tokens.Jwt;

namespace Carting.CustomMiddleware
{
    public class AccessTokenLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public AccessTokenLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].ToString();

            if (authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);

                    LogTokenDetails(jwtToken);
                }
            }

            await _next(context);
        }

        private void LogTokenDetails(JwtSecurityToken token)
        {
            //logging
        }
    }
}

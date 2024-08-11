using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class HttpJwtService
    {
        public void SetRefreshTokenCookie(HttpResponse response, string refreshToken, DateTime expiry)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiry
            };

            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        public string GetRefreshTokenFromCookies(HttpRequest request)
        {
            return request.Cookies["refreshToken"];
        }

        public void RemoveRefreshTokenCookie(HttpResponse response)
        {
            response.Cookies.Delete("refreshToken");
        }
    }
}

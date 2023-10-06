using System.Security.Claims;

namespace RUnGroopWebApp
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user) 
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}

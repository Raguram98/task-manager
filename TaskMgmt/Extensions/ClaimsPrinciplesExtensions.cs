using System.Security.Claims;

namespace TaskMgmt.Extensions
{
    public static class ClaimsPrinciplesExtensions
    {
        public static Guid GetUserId (this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("cannot get user ID from token"));
        }
    }
}

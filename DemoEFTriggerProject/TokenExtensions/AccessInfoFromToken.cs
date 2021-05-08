using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DemoEFTriggerProject.TokenExtensions
{
    public static class AccessInfoFromToken
    {
        public static int? GetMyUserId()
        {
            string userId = GlobalHttpContext._contextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                                                                                        .Select(x => x.Value)
                                                                                        .FirstOrDefault();

            if (userId is null)
                return null;

            return int.Parse(userId);
        }
    }
}

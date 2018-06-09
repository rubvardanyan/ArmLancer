using System.Linq;
using ArmLancer.Data.Models.Enums;

namespace ArmLancer.API.Utils.Attributes
{
    public class AuthorizeRoleAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public AuthorizeRoleAttribute(params UserRole[] roles)
        {
            var userRoles = string.Join(',', roles.Select(r => r.ToString()));
            this.Roles = userRoles;
        }
    }
}
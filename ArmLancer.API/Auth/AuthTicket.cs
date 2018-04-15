using ArmLancer.API.Utils.Settings;
using ArmLancer.Data.Models;

namespace ArmLancer.API.Auth
{
    public class AuthTicket
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public AuthTicket(User user, AuthSettings settings)
        {
            Token = AuthHelper.GenerateToken(user, settings);
            UserName = user.UserName;
            Role = user.Role.ToString();
        }
    }
}
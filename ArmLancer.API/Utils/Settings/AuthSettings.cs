namespace ArmLancer.API.Utils.Settings
{
    public class AuthSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInMins { get; set; }
    }
}
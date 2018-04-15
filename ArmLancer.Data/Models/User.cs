using ArmLancer.Data.Models.Enums;

namespace ArmLancer.Data.Models
{
    public class User : AbstractTrackingEntityModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        
        public virtual Client Client { get; set; }
    }
}
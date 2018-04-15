using System.ComponentModel.DataAnnotations;

namespace ArmLancer.API.Models.Requests
{
    public class RegisterRequest : AuthRequest
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        [PhoneAttribute]
        public string Phone { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
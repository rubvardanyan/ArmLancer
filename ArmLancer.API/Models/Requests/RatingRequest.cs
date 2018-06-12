using System.ComponentModel.DataAnnotations;

namespace ArmLancer.API.Models.Requests
{
    public class RatingRequest
    {
        [Required]
        public int Score { get; set; }

        [Required]
        public string Review { get; set; }
    }
}
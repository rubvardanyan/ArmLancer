using System.ComponentModel.DataAnnotations;

namespace ArmLancer.API.Models.Requests
{
    public class RatingRequest
    {
        [Required]
        [Range(1, 5)]
        public int Score { get; set; }

        [Required]
        public string Review { get; set; }
    }
}
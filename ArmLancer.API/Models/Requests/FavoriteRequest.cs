using System.ComponentModel.DataAnnotations;

namespace ArmLancer.API.Models.Requests
{
    public class FavoriteRequest
    {
        [Required]
        public long JobId { get; set; }
    }
}
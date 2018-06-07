using System.ComponentModel.DataAnnotations;
using ArmLancer.Data.Models.Enums;

namespace ArmLancer.API.Models.Requests
{
    public class JobRequest
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public JobDuration Duration { get; set; }
        
        [Required]
        public long CategoryId { get; set; }
    }
}
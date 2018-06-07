using System.ComponentModel.DataAnnotations;

namespace ArmLancer.API.Models.Requests
{
    public class JobSubmissionRequest
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public long JobId { get; set; }
        
    }
}
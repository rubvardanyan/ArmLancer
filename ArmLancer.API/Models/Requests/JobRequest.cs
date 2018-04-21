using ArmLancer.Data.Models.Enums;

namespace ArmLancer.API.Models.Requests
{
    public class JobRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public JobDuration Duration { get; set; }
    }
}
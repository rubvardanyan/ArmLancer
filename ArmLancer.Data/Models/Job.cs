using System.Collections.Generic;
using ArmLancer.Data.Models.Enums;

namespace ArmLancer.Data.Models
{
    public class Job
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public JobDuration Duration { get; set; }
        
        public long ClientId { get; set; }
        public virtual Client Client { get; set; }
        
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
        
        public virtual ICollection<JobSubmission> Submissions { get; set; }
    }
}
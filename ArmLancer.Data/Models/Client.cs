using System.Collections.Generic;

namespace ArmLancer.Data.Models
{
    public class Client : AbstractTrackingEntityModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        
        public long UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<JobSubmission> Submissions { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Rating> Rates { get; set; }
    }
}
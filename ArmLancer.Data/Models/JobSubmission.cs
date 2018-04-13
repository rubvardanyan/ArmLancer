using System.Dynamic;
using ArmLancer.Data.Models.Enums;

namespace ArmLancer.Data.Models
{
    public class JobSubmission
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public SubmissionStatus Status { get; set; }

        public long JobId { get; set; }
        public virtual Job Job { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
using ArmLancer.Data.Models.Enums;

namespace ArmLancer.API.Models.Responses
{
    public class JobSubmissionResponse
    {
        public long Id { get; set; }
        public long JobId { get; set; }
        public string Text { get; set; }
        public SubmissionStatus Status { get; set; }
    }
}
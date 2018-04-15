﻿using System.Dynamic;
using ArmLancer.Data.Models.Enums;

namespace ArmLancer.Data.Models
{
    public class JobSubmission : AbstractEntityModel
    {
        public string Text { get; set; }
        public SubmissionStatus Status { get; set; }

        public long JobId { get; set; }
        public virtual Job Job { get; set; }

        public long ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
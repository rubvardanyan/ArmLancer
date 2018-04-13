using System;

namespace ArmLancer.Data.Models
{
    public abstract class AbstractTrackingEntityModel
    {
        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime? Removed { get; set; }
    }
}
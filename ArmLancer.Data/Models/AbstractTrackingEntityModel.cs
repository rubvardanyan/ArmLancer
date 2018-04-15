using System;

namespace ArmLancer.Data.Models
{
    public abstract class AbstractTrackingEntityModel : AbstractEntityModel
    {
        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime? Removed { get; set; }
    }
}
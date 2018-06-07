namespace ArmLancer.Data.Models
{
    public class Favorite : AbstractEntityModel
    {
        public long ClientId { get; set; }
        public virtual Client Client { get; set; }
        
        public long JobId { get; set; }
        public virtual Job Job { get; set; }
        
    }
}
using System.Collections.Generic;

namespace ArmLancer.Data.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
        public long? ParentId { get; set; }
        
        public virtual Category ParentCategory {get;set;}

        public virtual ICollection<Category> Children { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
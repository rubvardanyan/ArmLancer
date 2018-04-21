using System.ComponentModel.DataAnnotations;

namespace ArmLancer.API.Models.Requests
{
    public class CategoryRequest
    {
        [Required]
        public string Name { get; set; }
        
        public long ParentId { get; set; }
    }
}
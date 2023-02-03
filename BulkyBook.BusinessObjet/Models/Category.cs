using System.ComponentModel.DataAnnotations;

namespace BulkyBook.BusinessObject.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 100, ErrorMessage = "Acceptable range is only from 1 to 100!")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NhaSachOnline.Models
{
    [Table("Genre")]
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? GenreName { get; set; }

        public List<Book> Books { get; set; }
    }
}
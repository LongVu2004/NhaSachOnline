using System.ComponentModel.DataAnnotations;

namespace NhaSachOnline.Models.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? GenreName { get; set; }
    }
}

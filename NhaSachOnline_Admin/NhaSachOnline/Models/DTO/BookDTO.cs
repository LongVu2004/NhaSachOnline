using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NhaSachOnline.Models.DTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? BookName { get; set; }
        [Required]
        [MaxLength(50)]
        public string? AuthorName { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá tiền phải là số và giá trị nguyên dương.")]
        public double Price { get; set; }
        public string? ImagePath { get; set; }
        [Required]
        public int GenreId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải là số và giá trị nguyên dương.")]
        public int Quantity { get; set; }
        public string GenreName { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<SelectListItem> GenreList { get; set; }
    }
}

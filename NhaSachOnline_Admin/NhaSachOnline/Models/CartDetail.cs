using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NhaSachOnline.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        public Book Book { get; set; }
        public Cart Cart { get; set; }
    }
}
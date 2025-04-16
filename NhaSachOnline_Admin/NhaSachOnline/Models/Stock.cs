using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NhaSachOnline.Models
{
    [Table("Stock")]
    public class Stock
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public Book? Book { get; set; }
    }
}

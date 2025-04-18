﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NhaSachOnline.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        // Email address đảm bảo rằng giá trị nhập vào từ giao diện là đúng format của 1 địa chỉ email
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string? MobileNumber { get; set; }

        [Required]
        [MaxLength(250)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string? PaymentMethod { get; set; }
        public bool isPaid { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
    }
}
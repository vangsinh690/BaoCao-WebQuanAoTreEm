using System.ComponentModel.DataAnnotations;

namespace BabyShop.Models
{
    public class NguoiDung
    {
        [Key] // Thiết lập maNguoiDung là Khóa chính
        public int maNguoiDung { get; set; }

        [Required]
        [StringLength(100)]
        public string hoTen { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        [Required]
        [StringLength(255)]
        public string matKhau { get; set; }

        [StringLength(50)]
        public string vaiTro { get; set; } // Admin hoặc Khách hàng
    }
}

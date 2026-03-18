using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyShop.Models
{
    // Dựa trên Đặc tả bảng ChiTietDonHang [2, 3]
    public class ChiTietDonHang
    {
        [Key]
        public int maChiTiet { get; set; } // Khóa chính [2]

        [Required]
        public int maDonHang { get; set; } // Khóa ngoài liên kết với DonHang [2]

        [ForeignKey("maDonHang")]
        public virtual DonHang DonHang { get; set; }

        [Required]
        public int maSanPham { get; set; } // Khóa ngoài liên kết với SanPham [2]

        [ForeignKey("maSanPham")]
        public virtual SanPham SanPham { get; set; }

        [Required]
        [Display(Name = "Số lượng")]
        public int soLuong { get; set; } // Số lượng sản phẩm khách đã mua [2]

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá lúc mua")]
        public decimal giaLucMua { get; set; } // Giá của sản phẩm tại thời điểm đặt hàng [2]
    }
}
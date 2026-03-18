using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyShop.Models
{
    public class SanPham
    {
        [Key] // Thiết lập maSanPham là Khóa chính
        public int maSanPham { get; set; }

        [Required]
        [StringLength(200)]
        public string tenSanPham { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal giaBan { get; set; }

        [StringLength(20)]
        public string kichCo { get; set; } // Size: S, M, L...

        [StringLength(50)]
        public string chatLieu { get; set; } // Cotton, Linen...

        public int soLuongTon { get; set; }

        // Khóa ngoại liên kết với bảng LoaiHang
        public int maLoaiHang { get; set; }

        [ForeignKey("maLoaiHang")]
        public virtual LoaiHang LoaiHang { get; set; }
    }
}
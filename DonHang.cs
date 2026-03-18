using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyShop.Models
{
    // Dựa trên Đặc tả bảng DonHang [2]
    public class DonHang
    {
        [Key]
        [Display(Name = "Mã đơn hàng")]
        public int maDonHang { get; set; } // Khóa chính [2]

        [Required]
        public int maNguoiDung { get; set; } // Khóa ngoài liên kết với NguoiDung [2]

        [ForeignKey("maNguoiDung")]
        public virtual NguoiDung NguoiDung { get; set; }

        [Required]
        [Display(Name = "Ngày đặt")]
        public DateTime ngayDat { get; set; } // Ngày khách hàng đặt hàng [2]

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền")]
        public decimal tongTien { get; set; } // Tổng giá trị đơn hàng [2]

        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string trangThai { get; set; } // Chờ duyệt, Đang giao, Thành công [2]

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Địa chỉ giao hàng")]
        public string diaChiGiaoHang { get; set; } // Địa chỉ nhận hàng của khách [2]

        // Liên kết với chi tiết đơn hàng
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
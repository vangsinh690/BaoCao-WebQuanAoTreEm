using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BabyShop.Models
{
    public class LoaiHang
    {
        [Key] // Thiết lập maLoaiHang là Khóa chính
        public int maLoaiHang { get; set; }

        [Required]
        [StringLength(100)]
        public string tenLoaiHang { get; set; }

        // Mối quan hệ: Một loại hàng có nhiều sản phẩm
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
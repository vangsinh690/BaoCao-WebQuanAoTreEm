namespace BabyShop.Models
{
    public class CartItem
    {
        public int maSanPham { get; set; }
        public string tenSanPham { get; set; }
        public decimal giaBan { get; set; }
        public int soLuong { get; set; }

        // CẬP NHẬT MỚI: Lưu trữ thuộc tính khách hàng lựa chọn theo Bảng 8 và Bảng 26
        public string sizeDaChon { get; set; } // Kích cỡ khách chọn (Ví dụ: S, M, L)
        public string mauDaChon { get; set; } // Màu sắc khách chọn (Ví dụ: Xanh, Hồng)

        // Tự động tính lại tổng tiền cho từng mặt hàng theo mô tả tại Bảng 27
        public decimal tongTien => soLuong * giaBan;
    }
}
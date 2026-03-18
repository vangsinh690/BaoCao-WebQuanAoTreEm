using Microsoft.AspNetCore.Mvc;
using BabyShop.Data;
using BabyShop.Models;
using BabyShop.Helpers;

namespace BabyShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Hiển thị trang Thanh toán (Luôn cho phép vào xem form theo Hình 26) [2], [3]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            if (cart == null || !cart.Any()) return RedirectToAction("Index", "Cart");

            ViewBag.TongTien = cart.Sum(i => i.tongTien);
            return View();
        }

        // 2. Xác nhận đặt hàng (Chỉ chặn khi nhấn nút này theo kịch bản lỗi Bảng 27) [3]
        [HttpPost]
        public IActionResult ProcessCheckout(string hoTen, string sdt, string diaChi, string phuongThucTT)
        {
            // BỔ SUNG: Kiểm tra đăng nhập khi bấm xác nhận
            var maNguoiDung = HttpContext.Session.GetInt32("maNguoiDung");

            if (maNguoiDung == null)
            {
                // Thông báo đúng nội dung bạn yêu cầu (Dựa trên Bảng 27) [3]
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để thanh toán";

                // Quay lại trang Checkout để hiện thông báo lỗi ngay trên các ô nhập liệu
                var cartItems = HttpContext.Session.Get<List<CartItem>>("GioHang");
                ViewBag.TongTien = cartItems?.Sum(i => i.tongTien);
                return View("Checkout");
            }

            // Nếu đã đăng nhập, tiến hành lưu như bình thường (Bảng 28) [3] 
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var donHang = new DonHang
            {
                maNguoiDung = maNguoiDung.Value,
                ngayDat = DateTime.Now,
                tongTien = cart.Sum(i => i.tongTien),
                trangThai = "Chờ duyệt",
                diaChiGiaoHang = $"{hoTen} - {sdt} - {diaChi}"
            };

            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                var ct = new ChiTietDonHang
                {
                    maDonHang = donHang.maDonHang,
                    maSanPham = item.maSanPham,
                    soLuong = item.soLuong,
                    giaLucMua = item.giaBan
                };
                _context.ChiTietDonHangs.Add(ct);
            }

            _context.SaveChanges();
            HttpContext.Session.Remove("GioHang"); // Làm trống giỏ hàng theo Bảng 9 [1]

            TempData["Success"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "Home");
        }
    }
}

using BabyShop.Data;
using BabyShop.Helpers;
using BabyShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Hiển thị danh sách giỏ hàng (Hiện thực hóa Bảng 27) [4]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang") ?? new List<CartItem>();

            // Tính tổng tiền thanh toán để hiển thị trên giao diện giỏ hàng [4]
            ViewBag.TongThanhToan = cart.Sum(item => item.tongTien);
            return View(cart);
        }

        // 2. Thêm vào giỏ hàng (Hiện thực hóa Bảng 8 và Bảng 26) [3, 5]
        [HttpPost]
        public IActionResult AddToCart(int maSanPham, string size, string mauSac, int soLuong = 1)
        {
            // Kiểm tra xem khách hàng đã chọn đầy đủ thuộc tính chưa theo Bảng 26 [3]
            if (string.IsNullOrEmpty(size) || string.IsNullOrEmpty(mauSac))
            {
                TempData["Error"] = "Vui lòng chọn đầy đủ Kích cỡ và Màu sắc!";
                return RedirectToAction("Details", "Home", new { id = maSanPham });
            }

            var sp = _context.SanPhams.Find(maSanPham);
            if (sp == null) return NotFound();

            // Kiểm tra số lượng tồn kho từ bảng SanPham (Bảng 18) [3, 6]
            if (sp.soLuongTon < soLuong)
            {
                TempData["Error"] = "Số lượng trong kho không đủ!";
                return RedirectToAction("Details", "Home", new { id = maSanPham });
            }

            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang") ?? new List<CartItem>();

            // Tìm sản phẩm trùng cả mã, size và màu sắc trong giỏ hàng hiện tại [3, 4]
            var item = cart.FirstOrDefault(p => p.maSanPham == maSanPham
                                            && p.sizeDaChon == size
                                            && p.mauDaChon == mauSac);

            if (item == null)
            {
                // Nếu chưa có, thêm mới một bản ghi vào danh sách chờ thanh toán [3]
                cart.Add(new CartItem
                {
                    maSanPham = sp.maSanPham,
                    tenSanPham = sp.tenSanPham,
                    giaBan = sp.giaBan,
                    soLuong = soLuong,
                    sizeDaChon = size, // Lưu kích cỡ đã chọn
                    mauDaChon = mauSac  // Lưu màu sắc đã chọn
                });
            }
            else
            {
                // Nếu đã có mặt hàng cùng loại/size/màu, chỉ cập nhật thêm số lượng [4]
                item.soLuong += soLuong;
            }

            // Lưu danh sách giỏ hàng vào Session (giỏ hàng tạm thời) [5]
            HttpContext.Session.Set("GioHang", cart);

            TempData["Success"] = "Đã thêm vào giỏ hàng thành công!";
            return RedirectToAction("Index");
        }

        // 3. Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int id, string size, string mauSac)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            if (cart != null)
            {
                // Tìm chính xác món đồ cần xóa dựa trên mã, size và màu
                var item = cart.FirstOrDefault(p => p.maSanPham == id
                                                && p.sizeDaChon == size
                                                && p.mauDaChon == mauSac);
                if (item != null) cart.Remove(item);

                HttpContext.Session.Set("GioHang", cart);
            }
            return RedirectToAction("Index");
        }
    }
}
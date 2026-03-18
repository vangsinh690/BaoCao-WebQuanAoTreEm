using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BabyShop.Data;
using BabyShop.Models;

namespace BabyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Chức năng: Hiển thị trang chủ với danh sách sản phẩm nổi bật
        // Hiện thực hóa theo mô tả tại Bảng 21 và Hình 19 [2], [3]
        public async Task<IActionResult> Index()
        {
            // Lấy toàn bộ danh sách sản phẩm kèm theo thông tin Loại hàng (Bé trai, bé gái...)
            var sanPhams = await _context.SanPhams.Include(s => s.LoaiHang).ToListAsync();
            return View(sanPhams);
        }

        // 2. Chức năng: Xem chi tiết sản phẩm
        // Hiện thực hóa theo mô tả tại Bảng 26 và Hình 24 [4], [3]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tìm sản phẩm theo mã định danh maSanPham (Khóa chính) [5]
            var sanPham = await _context.SanPhams
                .Include(s => s.LoaiHang)
                .FirstOrDefaultAsync(m => m.maSanPham == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // 3. CHỨC NĂNG MỚI: Tìm kiếm sản phẩm theo tên
        // Hiện thực hóa theo đặc tả tại Bảng 7 và Bảng 25 [1], [2]
        public async Task<IActionResult> Search(string searchTerm)
        {
            // Nếu người dùng không nhập từ khóa, hệ thống quay về Trang chủ
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            // Hệ thống thực hiện truy vấn SELECT với điều kiện LIKE trên trường tenSanPham [6]
            // Sử dụng .Contains() để tìm kiếm các sản phẩm có tên chứa từ khóa tìm kiếm
            var ketQua = await _context.SanPhams
                .Include(s => s.LoaiHang)
                .Where(s => s.tenSanPham.Contains(searchTerm))
                .ToListAsync();

            // Lưu từ khóa tìm kiếm vào ViewBag để hiển thị lại trên giao diện kết quả [6]
            ViewBag.SearchTerm = searchTerm;

            // Trả về View Tìm kiếm kèm theo danh sách sản phẩm khớp với từ khóa [7]
            return View(ketQua);
        }
    }
}
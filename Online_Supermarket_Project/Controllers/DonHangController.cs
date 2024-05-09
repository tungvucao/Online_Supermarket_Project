using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.ModelView;

namespace Online_Supermarket_Project.Controllers
{
    public class DonHangController : Controller
    {
        private readonly MyAppDbContext _context;

        public INotyfService _notyfService { get; set; }
        public DonHangController(MyAppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [HttpPost]        
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            try
            {
                var taikhoanId = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanId))
                {
                    return RedirectToAction("Login", "Accounts");
                }
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanId));
                if(khachhang == null) return NotFound();
                var donhang = await _context.Order.Include(x => x.TransactStatus).FirstOrDefaultAsync(x => x.OrderId == id && Convert.ToInt32(taikhoanId) == x.CusId);
                if(donhang == null) return NotFound();
                var chiTietDH = _context.OrderDetail.Include(x=>x.Product).AsNoTracking().Where(x=>x.OrderId==id).OrderBy(x=>x.OrderDetailId).ToList();
                XemDonHang xemDH = new XemDonHang();
                xemDH.DonHang = donhang;
                xemDH.ChiTietDonHang = chiTietDH;
                return PartialView("Details",xemDH);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Models;
using PagedList.Core;
using X.PagedList;
namespace Online_Supermarket_Project.Controllers
{
    public class OrderController : Controller
    {
        private readonly MyAppDbContext _db;
        private readonly ILogger<OrderController> _logger;
        public OrderController(MyAppDbContext db, ILogger<OrderController> logger)
        {
            _db = db;
            _logger = logger;
        }
        [HttpGet("/Cart")]
        public IActionResult Index()
        {
            var query = from a in _db.OrderDetail join 
                        b in _db.Product on a.ProductId equals b.ProductId
                        select new OrderDetailProduct{
                            ProductId = a.ProductId,
                            ProductName = b.ProductName,
                            OrderDetailId = a.OrderDetailId,
                            //ShortDesc = b.ShortDesc,
                            //Description = b.Description,
                            //CateId = b.CateId,
                            //Discount = b.Discount,
                            Image = b.Image,
                            //CreatedDate = b.CreatedDate,
                            //ModifiedDate = b.ModifiedDate,
                            //Status = b.Status,
                            //Title = b.Title,
                            //Alias = b.Alias,
                            //UnitsInStock = b.UnitsInStock,
                            Quantity = a.Quantity,
                            Total = a.Total,
                            Price = b.Price
                        };
            var model = query.ToList();
            return View(model);
        }
        public IActionResult XoaHang(int order)
        {
            _db.Remove(_db.OrderDetail.Single(a => a.OrderDetailId == order));
            _db.SaveChanges();
            return Json(new { success = true });
        }
    }
}

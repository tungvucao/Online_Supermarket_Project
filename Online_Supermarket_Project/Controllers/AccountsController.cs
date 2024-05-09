using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Extension;
using Online_Supermarket_Project.Helpper;
using Online_Supermarket_Project.Models;
using Online_Supermarket_Project.ModelView;
using System.Security.Claims;

namespace Online_Supermarket_Project.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly MyAppDbContext _context;

        public INotyfService _notyfService { get; set; }
        public AccountsController(MyAppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string phone)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == phone);
                if (khachhang != null) return Json(data: "Số Điện Thoại: " + phone + " đã được sử dụng!");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == email);
                if (khachhang != null) return Json(data: "Email: " + email + " đã được sử dụng!");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [Route("my-account.html", Name = "MyAccount")]
        public IActionResult Dashboard()
        {
            var taikhoanId = HttpContext.Session.GetString("CustomerId");
            if (taikhoanId != null)
            {
                var kh = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanId));
                if (kh != null) 
                {
                    var listOrder = _context.Order.Include(x => x.TransactStatus).AsNoTracking().Where(x => x.CusId == kh.CusId).OrderBy(x => x.OrderId).ToList();
                    ViewBag.listOrder = listOrder;
                    ViewBag.listCate = _context.Category.AsNoTracking().ToList();
                    return View(kh);
                } 
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> DangKy(Register customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    Customer tk = new Customer
                    {
                        FullName = customer.FullName,
                        Phone = customer.Phone,
                        Email = customer.Email,
                        Password = (customer.Password + salt.Trim()).ToMD5(),
                        Status = true,
                        Salt = salt,
                        CreatedDate = DateTime.Now
                    };
                    try
                    {
                        _context.Add(tk);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("CustomerId", tk.CusId.ToString());
                        var customerId = HttpContext.Session.GetString("CustomerId");

                        var claims = new List<Claim>
                        {
                            new Claim (ClaimTypes.Email, tk.Email),
                            new Claim ("CustomerId", tk.CusId.ToString())
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch
                    {
                        return RedirectToAction("DangKy", "Accounts");
                    }
                }
                else
                {
                    return View(customer);
                }
            }
            catch
            {
                return View(customer);
            }
        }

        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanId = HttpContext.Session.GetString("CustomerId");
            if (taikhoanId != null) return RedirectToAction("Dashboard", "Accounts");

            ViewBag.ReturnUrl = returnUrl;

            //ViewBag.ShoppingCarts = GioHang;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.ValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);

                    var kh = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName);
                    if (kh == null) 
                    {
                        _notyfService.Warning("Register if not have account");
                        return RedirectToAction("DangKy");
                    } 
                    string pass = (customer.Password + kh.Salt.Trim()).ToMD5();
                    if (kh.Password != pass)
                    {
                        _notyfService.Success("Thoong tin không chính xác!");
                        return View(customer);
                    }

                    if (kh.Status == false) return RedirectToAction("ThongBao", "Accounts");
                      
                    HttpContext.Session.SetString("CustomerId", kh.CusId.ToString());
                    var taikhoanId = HttpContext.Session.GetString("CustomerId");

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.FullName),
                        new Claim("CustomerId",kh.CusId.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _notyfService.Success("Welcome!");
                    kh.LastLogin = DateTime.Now;
                    _context.Update(kh);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard", "Accounts");
                }
            }
            catch
            {
                return RedirectToAction("DangKy", "Accounts");
            }
            return View(customer);
        }

        [HttpGet]
        [Route("dang-xuat.html", Name = "Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {
            try
            {
                var cusId = int.Parse(HttpContext.Session.GetString("CustomerId"));
                if(cusId == null) return RedirectToAction("Login", "Accounts");

                if (ModelState.IsValid)
                {
                    var customer = _context.Customers.Find(cusId);
                    if (customer == null) return RedirectToAction("Index", "Home");

                    var pass = (model.PasswordNow.Trim() + customer.Salt.Trim()).ToMD5();
                    if (pass == customer.Password)
                    {
                        string passNew = (model.Password.Trim() + customer.Salt.Trim()).ToMD5();
                        customer.Password = passNew;
                        _context.Update(customer);
                        _context.SaveChanges();
                        _notyfService.Success("Change Password Success!");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                }
            }
            catch 
            {
                _notyfService.Warning("Change Password Error!");
                return RedirectToAction("Dashboard", "Accounts");
            }
            _notyfService.Warning("Change Password Error!");
            return RedirectToAction("Dashboard", "Accounts");
        }
    }
}

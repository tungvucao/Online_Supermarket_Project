using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Supermarket_Project.AppContext;
using Online_Supermarket_Project.Helpper;
using Online_Supermarket_Project.Models;
using PagedList.Core;

namespace Online_Supermarket_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminNewsController : Controller
    {
        private readonly MyAppDbContext _context;

        public INotyfService _notyfService { get; set; }
        public AdminNewsController(MyAppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminNews
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;
            var listNews = _context.New
                .AsNoTracking()
                .OrderBy(p => p.NewId);

            PagedList<New> models = new PagedList<New>(listNews, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @new = await _context.New
                .FirstOrDefaultAsync(m => m.NewId == id);
            if (@new == null)
            {
                return NotFound();
            }

            return View(@new);
        }

        // GET: Admin/AdminNews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection formValues, Microsoft.AspNetCore.Http.IFormFile? fImage, [Bind("NewId,Title,ShortContent,Content,Thumb,Published,Alias,CreatedDate,Author,AccountId,Tags,CateId,IsHot,IsNewFeed,MetaKey,MetaDesc,Views")] New news)
        {
            if (ModelState.IsValid)
            {
                news.Title = Utilities.ToTitleCase(news.Title);
                if (fImage != null)
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "png" };
                    var fileExt = Path.GetExtension(fImage.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt.ToLower()))
                    {
                        _notyfService.Warning("Supported Types Image: jpg,jpeg,png! Please choose another image.");
                        return View();
                    }
                    string extension = Path.GetExtension(fImage.FileName);
                    string image = Utilities.ToUrlFriendly(fImage.FileName) + extension;
                    news.Thumb = await Utilities.UploadFile(fImage, @"news", image.ToLower());
                }
                else
                {
                    news.Thumb = "default.jpg";
                }
                if (string.IsNullOrEmpty(news.Thumb)) news.Thumb = "default.jpg";
                news.Content = formValues["editor"];
                news.CreatedDate = DateTime.Now;
                news.Alias = Utilities.ToUrlFriendly(news.Title);
                _context.Add(news);
                _notyfService.Success("Create Success");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: Admin/AdminNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @new = await _context.New.FindAsync(id);
            if (@new == null)
            {
                return NotFound();
            }
            return View(@new);
        }

        // POST: Admin/AdminNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection formValues, Microsoft.AspNetCore.Http.IFormFile? fImage, int id, [Bind("NewId,Title,ShortContent,Content,Thumb,Published,Alias,CreatedDate,Author,AccountId,Tags,CateId,IsHot,IsNewFeed,MetaKey,MetaDesc,Views")] New news)
        {
            if (id != news.NewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    news.Title = Utilities.ToTitleCase(news.Title);
                    if (fImage != null)
                    {
                        var supportedTypes = new[] { "jpg", "jpeg", "png" };
                        var fileExt = Path.GetExtension(fImage.FileName).Substring(1);
                        if (!supportedTypes.Contains(fileExt.ToLower()))
                        {
                            _notyfService.Warning("Supported Types Image: jpg,jpeg,png! Please choose another image.");
                            return View(news);
                        }
                        string extension = Path.GetExtension(fImage.FileName);
                        string image = Utilities.ToUrlFriendly(fImage.FileName) + extension;
                        news.Thumb = await Utilities.UploadFile(fImage, @"news", image.ToLower());
                    }
                    else
                    {
                        news.Thumb = "default.jpg";
                    }
                    if (string.IsNullOrEmpty(news.Thumb)) news.Thumb = "default.jpg";
                    news.Content = formValues["editor"];
                    news.Alias = Utilities.ToUrlFriendly(news.Title);
                    _notyfService.Success("Cập nhật thành công");
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewExists(news.NewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: Admin/AdminNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @new = await _context.New
                .FirstOrDefaultAsync(m => m.NewId == id);
            if (@new == null)
            {
                return NotFound();
            }

            return View(@new);
        }

        // POST: Admin/AdminNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @new = await _context.New.FindAsync(id);
            if (@new != null)
            {
                _context.New.Remove(@new);
                _notyfService.Success("Delete Success");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewExists(int id)
        {
            return _context.New.Any(e => e.NewId == id);
        }
    }
}

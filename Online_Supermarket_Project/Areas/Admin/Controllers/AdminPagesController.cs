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
    public class AdminPagesController : Controller
    {
        private readonly MyAppDbContext _context;

        public INotyfService _notyfService { get; set; }
        public AdminPagesController(MyAppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminPages
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;
            var listPage = _context.Page
                .AsNoTracking()
                .OrderBy(p => p.PageId);
            PagedList<Page> models = new PagedList<Page>(listPage, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Page
                .FirstOrDefaultAsync(m => m.PageId == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: Admin/AdminPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection formValues, Microsoft.AspNetCore.Http.IFormFile fImage, [Bind("PageId,PageName,Contents,Thumb,Published,Title,MetaDesc,MetaKey,Alias,CreatedDate,Ordering")] Page page)
        {
            if (fImage == null || fImage.Length == 0)
            {
                _notyfService.Warning("Choose any image!");
                return View();

            }
            if (ModelState.IsValid)
            {
                page.PageName = Utilities.ToTitleCase(page.PageName);
                if (fImage != null)
                {
                    string extension = Path.GetExtension(fImage.FileName);
                    string image = Utilities.ToUrlFriendly(fImage.FileName) + extension;
                    page.Thumb = await Utilities.UploadFile(fImage, @"pages", image.ToLower());
                }

                page.Contents = formValues["editor"];
                page.CreatedDate = DateTime.Now;
                _context.Add(page);
                _notyfService.Success("Create Success");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(page);
        }

        // GET: Admin/AdminPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Page.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        // POST: Admin/AdminPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection formValues, Microsoft.AspNetCore.Http.IFormFile? fImage, int id, [Bind("PageId,PageName,Contents,Thumb,Published,Title,MetaDesc,MetaKey,Alias,CreatedDate,Ordering")] Page page)
        {
            if (id != page.PageId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (fImage != null)
                    {
                        string extension = Path.GetExtension(fImage.FileName);
                        string image = Utilities.ToUrlFriendly(fImage.FileName) + extension;
                        page.Thumb = await Utilities.UploadFile(fImage, @"pages", image.ToLower());
                    }

                    page.Contents = formValues["editor"];
                    _context.Update(page);
                    _notyfService.Success("Update Success");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.PageId))
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
            return View(page);
        }

        // GET: Admin/AdminPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Page
                .FirstOrDefaultAsync(m => m.PageId == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Admin/AdminPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var page = await _context.Page.FindAsync(id);
            if (page != null)
            {
                _context.Page.Remove(page);
                _notyfService.Success("Delete Success");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageExists(int id)
        {
            return _context.Page.Any(e => e.PageId == id);
        }
    }
}

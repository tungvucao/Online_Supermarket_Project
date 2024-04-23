﻿using System;
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
    public class AdminProductsController : Controller
    {
        private readonly MyAppDbContext _context;
        public INotyfService _notyfService { get; set; }
        public AdminProductsController(MyAppDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int? page, int CateId = 0)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;

            List<Product> lsProducts = new List<Product>();

            if (CateId != 0)
            {
                lsProducts = _context.Product
                .AsNoTracking()
                .Where(x => x.CateId == CateId)
                .Include(p => p.Cate)
                .ToList();
            }
            else
            {
                lsProducts = _context.Product
                .AsNoTracking()
                .Include(p => p.Cate)
                .ToList();
            }

            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentCateId = CateId;
            ViewData["Category"] = new SelectList(_context.Category, "CateId", "CateName", CateId);

            return View(models);
        }

        public IActionResult Filter(int CateId = 0)
        {
            var url = $"/Admin/AdminProducts?CateId={CateId}";
            if(CateId == 0)
            {
                url = $"/Admin/AdminProducts";
            }

            return Json(new { status = "success", redirectUrl = url});
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_context.Category, "CateId", "CateName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection formValues, Microsoft.AspNetCore.Http.IFormFile fImage, [Bind("ProductId,ProductName,ShortDesc,Description,CateId,Price,Discount,Image,CreatedDate,ModifiedDate,Status,Title,Alias,UnitsInStock")] Product product)
        {
            if (fImage == null || fImage.Length == 0)
            {
                _notyfService.Warning("Choose any image!");
                ViewData["Category"] = new SelectList(_context.Category, "CateId", "CateName", product.CateId);
                return View();

            }
            if (product.CateId == null )
            {
                _notyfService.Warning("Choose any category!");
                ViewData["Category"] = new SelectList(_context.Category, "CateId", "CateName", product.CateId);
                return View();
            }
            if (ModelState.IsValid)
            {
                product.ProductName = Utilities.ToTitleCase(product.ProductName);
                
                if (fImage != null)
                {
                    string extension = Path.GetExtension(fImage.FileName);
                    string image = Utilities.ToUrlFriendly(fImage.FileName) + extension;
                    product.Image = await Utilities.UploadFile(fImage, @"products", image.ToLower());
                }
                product.Alias = Utilities.ToUrlFriendly(product.ProductName);
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                product.Description = formValues["editor"];
                _context.Add(product);
                _notyfService.Success("Create Successfully!");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(_context.Category, "CateId", "CateName", product.CateId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_context.Category, "CateId", "CateName", product.CateId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Microsoft.AspNetCore.Http.IFormFile? fImage, int id, IFormCollection formValues, [Bind("ProductId,ProductName,ShortDesc,Description,CateId,Price,Discount,Image,CreatedDate,ModifiedDate,Status,Title,Alias,UnitsInStock")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            if (fImage == null || fImage.Length == 0)
            {
                _notyfService.Warning("Choose any image!");
                ViewData["Category"] = new SelectList(_context.Category, "CateId", "CateName", product.CateId);
                return View(product);

            }
            if (product.CateId == null)
            {
                _notyfService.Warning("Choose any category!");
                ViewData["Category"] = new SelectList(_context.Category, "CateId", "CateName", product.CateId);
                return View(product);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    product.ProductName = Utilities.ToTitleCase(product.ProductName);
                    product.Alias = Utilities.ToUrlFriendly(product.ProductName);
                    product.ModifiedDate = DateTime.Now;
                    product.Description = formValues["editor"];
                    
                    if (fImage != null && fImage.Length >0)
                    {
                        if (!string.IsNullOrEmpty(product.Image) || product.Image != null)
                        {
                            //delete old image
                            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", @"products", product.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        string extension = Path.GetExtension(fImage.FileName);
                        string image = Utilities.ToUrlFriendly(fImage.FileName) + extension;
                        product.Image = await Utilities.UploadFile(fImage, @"products", image.ToLower());
                    }
                    _context.Update(product);
                    _notyfService.Success("Update Successfully!");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _notyfService.Error("Concurrency error occurred.");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(_context.Category, "CateId", "CateName", product.CateId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _notyfService.Success("Delete Successfully!");
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
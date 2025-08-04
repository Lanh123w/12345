using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class FoodItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FoodItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //public IActionResult Index()
        //{
        //    var items = _context.FoodItems.ToList();
        //    return View(items);
        //}

        // POST: Tạo món ăn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodItem item, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    item.ImageUrl = "/images/" + fileName;
                }

                _context.FoodItems.Add(item);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "✅ Đã thêm món ăn!";
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        // GET: Sửa món ăn
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.FoodItems.FindAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: Sửa món ăn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodItem item, IFormFile ImageFile)
        {
            if (id != item.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.FoodItems.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
                if (existing == null) return NotFound();

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    item.ImageUrl = "/images/" + fileName;
                }
                else
                {
                    item.ImageUrl = existing.ImageUrl;
                }

                _context.FoodItems.Update(item);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "✅ Đã sửa món ăn!";
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        // GET: Xóa món ăn
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.FoodItems.FindAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: Xác nhận xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.FoodItems.FindAsync(id);
            if (item != null)
            {
                _context.FoodItems.Remove(item);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "✅ Đã xóa món ăn!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /FoodItems
        public async Task<IActionResult> Index()
        {
            var items = await _context.FoodItems.Include(f => f.Category).ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // Hiển thị món ăn theo danh mục
        public async Task<IActionResult> ItemsByCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.FoodItems)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }
        public async Task<IActionResult> AllItems()
        {
            var allItems = await _context.FoodItems
                .Include(f => f.Category)
                .ToListAsync();

            return View(allItems);
        }

    }
}

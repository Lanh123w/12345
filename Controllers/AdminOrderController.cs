using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

[Authorize(Roles = "Admin")]
public class AdminOrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminOrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(d => d.FoodItem)
            .Select(o => new Order
            {
                Id = o.Id,
                CustomerName = o.CustomerName ?? "Không có tên",
                Address = o.Address ?? "Chưa có địa chỉ",
                Phone = o.Phone ?? "Chưa có số",
                OrderDate = o.OrderDate ,
                Status = o.Status ?? "Chưa xác định",
                TotalAmount = o.TotalAmount ,
                OrderDetails = o.OrderDetails
            })
            .ToListAsync();

        return View(orders);
    }

}

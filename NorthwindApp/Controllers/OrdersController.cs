using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindApp.Data;
using NorthwindApp.Models;

namespace NorthwindApp.Controllers;

[Authorize(Roles = "Admin")]
public class OrdersController : Controller
{
    private readonly NorthwindContext _context;

    public OrdersController(NorthwindContext context)
    {
        _context = context;
    }

    // GET: Orders - Consulta LINQ #7: Los 5 pedidos más recientes
    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .ToListAsync();
        return View(orders);
    }

    // GET: Orders/Details/5
    public async Task<IActionResult> Details(short? id)
    {
        if (id == null) return NotFound();

        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(m => m.OrderId == id);

        if (order == null) return NotFound();
        return View(order);
    }
}

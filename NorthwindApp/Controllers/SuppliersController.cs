using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindApp.Data;
using NorthwindApp.Models;

namespace NorthwindApp.Controllers;

public class SuppliersController : Controller
{
    private readonly NorthwindContext _context;

    public SuppliersController(NorthwindContext context)
    {
        _context = context;
    }

    // GET: Suppliers
    public async Task<IActionResult> Index()
    {
        var suppliers = await _context.Suppliers
            .OrderBy(s => s.CompanyName)
            .ToListAsync();
        return View(suppliers);
    }

    // GET: Suppliers/Details/5
    public async Task<IActionResult> Details(short? id)
    {
        if (id == null) return NotFound();

        var supplier = await _context.Suppliers
            .Include(s => s.Products)
            .FirstOrDefaultAsync(m => m.SupplierId == id);

        if (supplier == null) return NotFound();
        return View(supplier);
    }
}

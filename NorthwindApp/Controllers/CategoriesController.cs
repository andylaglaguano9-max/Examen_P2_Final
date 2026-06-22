using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindApp.Data;
using NorthwindApp.Models;

namespace NorthwindApp.Controllers;

public class CategoriesController : Controller
{
    private readonly NorthwindContext _context;

    public CategoriesController(NorthwindContext context)
    {
        _context = context;
    }

    // GET: Categories
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .OrderBy(c => c.CategoryName)
            .ToListAsync();
        return View(categories);
    }

    // GET: Categories/Details/5
    public async Task<IActionResult> Details(short? id)
    {
        if (id == null) return NotFound();

        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(m => m.CategoryId == id);

        if (category == null) return NotFound();
        return View(category);
    }
}

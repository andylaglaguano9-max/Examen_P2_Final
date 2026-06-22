using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NorthwindApp.Data;
using NorthwindApp.Models;

namespace NorthwindApp.Controllers;

public class ProductsController : Controller
{
    private readonly NorthwindContext _context;

    public ProductsController(NorthwindContext context)
    {
        _context = context;
    }

    // GET: Products - Vista relacional con Include()
    public async Task<IActionResult> Index()
    {
        // Consulta LINQ #3: 10 productos con nombre de categoría usando Include
        var productos = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .OrderBy(p => p.ProductName)
            .Take(10)
            .ToListAsync();
        return View(productos);
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(short? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(m => m.ProductId == id);

        if (product == null) return NotFound();
        return View(product);
    }

    // GET: Products/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName");
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", product.SupplierId);
        return View(product);
    }

    // GET: Products/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(short? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", product.SupplierId);
        return View(product);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(short id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
    {
        if (id != product.ProductId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", product.SupplierId);
        return View(product);
    }

    // GET: Products/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(short? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(m => m.ProductId == id);

        if (product == null) return NotFound();
        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(short id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null) _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // ========== CONSULTAS LINQ ==========

    // Consulta LINQ #1: Los 10 productos más caros (OrderByDescending + Take)
    public async Task<IActionResult> MasCaros()
    {
        var productos = await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.UnitPrice)
            .Take(10)
            .ToListAsync();
        return View(productos);
    }

    // Consulta LINQ #2: Productos cuyo nombre contenga "Ch" (Where con condición)
    public async Task<IActionResult> BuscarPorNombre()
    {
        string palabraBusqueda = "Ch";
        var productos = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.ProductName.Contains(palabraBusqueda))
            .OrderBy(p => p.ProductName)
            .ToListAsync();
        ViewData["PalabraBusqueda"] = palabraBusqueda;
        return View(productos);
    }

    // Consulta LINQ #4: 10 productos con nombre de proveedor usando Include
    public async Task<IActionResult> ProductosPorProveedor()
    {
        var productos = await _context.Products
            .Include(p => p.Supplier)
            .OrderBy(p => p.ProductName)
            .Take(10)
            .ToListAsync();
        return View(productos);
    }

    // Consulta LINQ #5: Productos de una categoría específica usando Join
    public async Task<IActionResult> ProductosPorCategoria()
    {
        var resultado = await (from p in _context.Products
                               join c in _context.Categories on p.CategoryId equals c.CategoryId
                               where c.CategoryName == "Beverages"
                               orderby p.ProductName
                               select new ProductoCategoriaViewModel
                               {
                                   ProductName = p.ProductName,
                                   UnitPrice = p.UnitPrice,
                                   UnitsInStock = p.UnitsInStock,
                                   CategoryName = c.CategoryName
                               }).ToListAsync();
        return View(resultado);
    }

    // Consulta LINQ #6: Productos de un proveedor específico (Where + condición compuesta)
    public async Task<IActionResult> ProductosProveedorEspecifico()
    {
        var productos = await _context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Category)
            .Where(p => p.Supplier != null && p.Supplier.Country == "USA" && p.UnitPrice > 10)
            .OrderByDescending(p => p.UnitPrice)
            .ToListAsync();
        return View(productos);
    }

    private bool ProductExists(short id)
    {
        return _context.Products.Any(e => e.ProductId == id);
    }
}

// ViewModel para la consulta con Join
public class ProductoCategoriaViewModel
{
    public string ProductName { get; set; } = null!;
    public float? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
    public string CategoryName { get; set; } = null!;
}

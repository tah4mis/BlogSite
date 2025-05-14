using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogSite.Models;
using BlogSite.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlogContext _context;

    public HomeController(ILogger<HomeController> logger, BlogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Son eklenen 5 makaleyi al
        ViewBag.RecentArticles = await _context.Articles
            .Include(a => a.Author)
            .Include(a => a.Category)
            .OrderByDescending(a => a.PublishedAt)
            .Take(5)
            .ToListAsync();

        // Tüm kategorileri ve makale sayılarını al
        ViewBag.Categories = await _context.Categories
            .Include(c => c.Articles)
            .ToListAsync();

        // İstatistikler
        ViewBag.TotalArticles = await _context.Articles.CountAsync();
        ViewBag.TotalAuthors = await _context.Users.CountAsync();

        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

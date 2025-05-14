using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogSite.Data;
using BlogSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BlogSite.Controllers
{
    public class ArticleController : Controller
    {
        private readonly BlogContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ArticleController> _logger;
        private readonly int PageSize = 5;

        public ArticleController(BlogContext context, UserManager<User> userManager, ILogger<ArticleController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString, string category, int page = 1)
        {
            var query = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(a => 
                    a.Title.Contains(searchString) || 
                    a.Content.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(a => a.Category.Name == category);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            var articles = await query
                .OrderByDescending(a => a.PublishedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.Category = category;
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(articles);
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(article);
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı. Lütfen tekrar giriş yapın.");
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(article);
                }

                article.UserId = user.Id;
                article.PublishedAt = DateTime.Now;

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Makale eklenirken hata oluştu: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Makale eklenirken bir hata oluştu.");
                ViewBag.Categories = _context.Categories.ToList();
                return View(article);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            // Admin her makaleyi düzenleyebilir
            if (!User.IsInRole("Admin") && article.Author?.UserName != User.Identity?.Name)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(article);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            var existingArticle = await _context.Articles
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingArticle == null)
            {
                return NotFound();
            }

            // Admin her makaleyi düzenleyebilir
            if (!User.IsInRole("Admin") && existingArticle.Author?.UserName != User.Identity?.Name)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Sadece belirli alanları güncelle
                    existingArticle.Title = article.Title;
                    existingArticle.Content = article.Content;
                    existingArticle.CategoryId = article.CategoryId;

                    _context.Update(existingArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
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
            ViewBag.Categories = _context.Categories.ToList();
            return View(article);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            var currentUserId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            if (article.UserId != currentUserId)
            {
                return Forbid();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
} 
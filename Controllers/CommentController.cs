using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogSite.Data;
using BlogSite.Models;

namespace BlogSite.Controllers
{
    public class CommentController : Controller
    {
        private readonly BlogContext _context;

        public CommentController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int articleId, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest("Comment content cannot be empty");
            }

            var article = await _context.Articles.FindAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Content = content,
                ArticleId = articleId,
                CreatedAt = DateTime.UtcNow,
                // UserId will be set when authentication is implemented
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Article", new { id = articleId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Article", new { id = comment.ArticleId });
        }
    }
} 
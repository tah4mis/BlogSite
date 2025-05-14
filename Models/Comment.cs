using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public string? UserId { get; set; }
        public User? User { get; set; }
    }
} 
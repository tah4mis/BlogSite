using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSite.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200)]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik zorunludur")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;

        public DateTime PublishedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kategori seçmelisiniz")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [ForeignKey("UserId")]
        public User? Author { get; set; }

        public List<Comment> Comments { get; set; } = new();
    }
} 
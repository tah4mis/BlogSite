using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
} 
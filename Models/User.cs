using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BlogSite.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
} 
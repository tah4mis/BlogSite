using System;
using System.Linq;
using System.Threading.Tasks;
using BlogSite.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogSite.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(BlogContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Kategorileri kontrol et
            if (!context.Categories.Any())
            {
                var categories = new Category[]
                {
                    new Category { Name = "Technology" },
                    new Category { Name = "Software Development" },
                    new Category { Name = "Artificial Intelligence" },
                    new Category { Name = "Web Development" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Admin rolünü oluştur
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Test kullanıcısını kontrol et
            if (!context.Users.Any())
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Test makalelerini kontrol et
            if (!context.Articles.Any())
            {
                var user = await userManager.FindByNameAsync("admin");
                var categories = context.Categories.ToList();

                var articles = new Article[]
                {
                    new Article
                    {
                        Title = "Getting Started with ASP.NET Core",
                        Content = "ASP.NET Core is a cross-platform, high-performance, open-source framework for building modern, cloud-enabled, Internet-connected applications. This article will cover the basic concepts and features of ASP.NET Core.",
                        PublishedAt = DateTime.UtcNow,
                        UserId = user.Id,
                        CategoryId = categories.First(c => c.Name == "Web Development").Id
                    },
                    new Article
                    {
                        Title = "Introduction to AI and Machine Learning",
                        Content = "Artificial Intelligence and Machine Learning are among the fastest-growing fields in technology today. In this article, we'll explore the fundamental concepts and applications of AI and ML.",
                        PublishedAt = DateTime.UtcNow.AddDays(-1),
                        UserId = user.Id,
                        CategoryId = categories.First(c => c.Name == "Artificial Intelligence").Id
                    }
                };

                context.Articles.AddRange(articles);
                await context.SaveChangesAsync();
            }
        }
    }
} 
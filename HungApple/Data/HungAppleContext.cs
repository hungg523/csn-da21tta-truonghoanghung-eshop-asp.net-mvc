using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HungApple.Models;

namespace HungApple.Data
{
    public class HungAppleContext : DbContext
    {
        public HungAppleContext (DbContextOptions<HungAppleContext> options)
            : base(options)
        {
        }

        public DbSet<HungApple.Models.Category> Category { get; set; } = default!;

        public DbSet<HungApple.Models.Contact>? Contact { get; set; }

        public DbSet<HungApple.Models.Product>? Product { get; set; }

        public DbSet<HungApple.Models.User>? User { get; set; }

        public DbSet<HungApple.Models.Comment>? Comment { get; set; }
        public DbSet<HungApple.Models.Provinces>? Provinces { get; set; }
        public DbSet<HungApple.Models.Districts>? Districts { get; set; }
        public DbSet<HungApple.Models.Wards>? Wards { get; set; }
        public DbSet<HungApple.Models.Payment>? Delivery { get; set; }
        public DbSet<HungApple.Models.Order>? Order { get; set; }
        public DbSet<HungApple.Models.NewsLetterSubscription>? NewsLetterSubscription { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using UrlShorteningService.Models;

namespace UrlShorteningService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}
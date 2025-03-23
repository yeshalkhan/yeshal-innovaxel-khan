using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShorteningService.Data;
using UrlShorteningService.Models;

namespace UrlShorteningService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortUrlController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShortUrlController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GenerateShortCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }

        [HttpGet("shorten/{shortCode}")]
        public async Task<IActionResult> GetOriginalUrl(string shortCode)
        {
            var shortUrl = await _context.ShortUrls.FirstOrDefaultAsync(s => s.ShortCode == shortCode);
            if (shortUrl == null)
                return NotFound(new { error = "URL not found" });

            shortUrl.AccessCount++;
            await _context.SaveChangesAsync();

            return Ok(shortUrl);
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> CreateShortUrl(string url)
        {
            var shortCode = GenerateShortCode();
            var shortUrl = new ShortUrl
            {
                Url = url,
                ShortCode = shortCode
            };

            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOriginalUrl), new { shortCode }, shortUrl);
        }

    }
}

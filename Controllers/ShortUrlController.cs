using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
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

        private bool IsValidUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
                return false;

            string host = uriResult.Host;

            string domainPattern = @"^([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$";

            return Regex.IsMatch(host, domainPattern);
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
        public async Task<IActionResult> CreateShortUrl([FromBody] UrlRequestDto urlRequest)
        {
            if (string.IsNullOrWhiteSpace(urlRequest.Url) || !IsValidUrl(urlRequest.Url))
                return BadRequest(new { error = "Invalid URL format" });

            var shortCode = GenerateShortCode();
            var shortUrl = new ShortUrl
            {
                Url = urlRequest.Url,
                ShortCode = shortCode
            };

            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOriginalUrl), new { shortCode }, shortUrl);
        }

        [HttpPut("shorten/{shortCode}")]
        public async Task<IActionResult> UpdateShortUrl(string shortCode, [FromBody] UrlRequestDto urlRequest)
        {
            var shortUrl = await _context.ShortUrls.FirstOrDefaultAsync(s => s.ShortCode == shortCode);
            if (shortUrl == null)
                return NotFound(new { error = "Short URL not found" });

            if (string.IsNullOrWhiteSpace(urlRequest.Url) || !IsValidUrl(urlRequest.Url))
                return BadRequest(new { error = "Invalid URL format" });

            shortUrl.Url = urlRequest.Url;
            shortUrl.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(shortUrl);
        }

        [HttpDelete("shorten/{shortCode}")]
        public async Task<IActionResult> DeleteShortUrl(string shortCode)
        {
            var shortUrl = await _context.ShortUrls.FirstOrDefaultAsync(s => s.ShortCode == shortCode);
            if (shortUrl == null)
                return NotFound(new { error = "Short URL not found" });

            _context.ShortUrls.Remove(shortUrl);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("shorten/{shortCode}/stats")]
        public async Task<IActionResult> GetUrlStats(string shortCode)
        {
            var shortUrl = await _context.ShortUrls.FirstOrDefaultAsync(s => s.ShortCode == shortCode);
            if (shortUrl == null)
                return NotFound(new { error = "Short URL not found" });

            return Ok(shortUrl);
        }
    }
}

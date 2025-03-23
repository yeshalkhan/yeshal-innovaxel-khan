﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<IActionResult> CreateShortUrl([FromBody] UrlRequestDto urlRequest)
        {
            if (string.IsNullOrEmpty(urlRequest.Url))
                return BadRequest(new { error = "URL is required" });

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
            if (string.IsNullOrEmpty(urlRequest.Url))
                return BadRequest(new { error = "URL is required" });

            var shortUrl = await _context.ShortUrls.FirstOrDefaultAsync(s => s.ShortCode == shortCode);
            if (shortUrl == null)
                return NotFound(new { error = "Short URL not found" });

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

namespace UrlShorteningService.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int AccessCount { get; set; } = 0;
    }
}

using Microsoft.AspNetCore.Mvc;
using UrlShorteningService.Data;

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
    }
}

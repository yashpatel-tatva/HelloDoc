using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Controllers
{
    public class CredentialController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CredentialController(ApplicationDbContext context) { 
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Login(Region region)
        {
            var correct = await _context.Regions
                .FirstOrDefaultAsync(m => m.Name == region.Name);
            if(correct.Abbreviation == region.Abbreviation) {
                return View("Correct");
            }
            return View("hello");
        }
    }
}

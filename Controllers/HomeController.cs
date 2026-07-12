using CV_Builder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CV_Builder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var personalInfo = await _context.PersonalInfos.FirstOrDefaultAsync();
            var projects = await _context.Projects.OrderBy(p => p.DisplayOrder).ToListAsync();
            var educations = await _context.Educations.ToListAsync();
            var skills = await _context.Skills.ToListAsync();

            var model = new DashboardViewModel
            {
                PersonalInfo = personalInfo,
                Projects = projects,
                Educations = educations,
                Skills = skills
            };
            return View(model);
        }
    }
}
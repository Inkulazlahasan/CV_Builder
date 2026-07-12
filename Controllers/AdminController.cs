using CV_Builder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CV_Builder.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============ LOGIN ============
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string password)
        {
            if (password == "admin123") // Change this password later!
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Dashboard");
            }
            ViewBag.Error = "Wrong password!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ============ DASHBOARD ============
        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            var model = new DashboardViewModel
            {
                PersonalInfo = await _context.PersonalInfos.FirstOrDefaultAsync(),
                Projects = await _context.Projects.OrderBy(p => p.DisplayOrder).ToListAsync(),
                Educations = await _context.Educations.OrderBy(e => e.DisplayOrder).ToListAsync(), // Sorted
                Skills = await _context.Skills.OrderBy(s => s.DisplayOrder).ToListAsync()          // Sorted
            };
            return View(model);
        }

        // ============ EDIT PERSONAL INFO ============
        public async Task<IActionResult> EditPersonalInfo()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            var info = await _context.PersonalInfos.FirstOrDefaultAsync();
            if (info == null)
            {
                info = new PersonalInfo();
                _context.PersonalInfos.Add(info);
                await _context.SaveChangesAsync();
            }
            return View(info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPersonalInfo(PersonalInfo model, IFormFile photoFile)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                // Handle photo upload
                if (photoFile != null && photoFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images"));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(stream);
                    }

                    model.PhotoPath = "/images/" + fileName;
                }
                else
                {
                    // Keep existing photo if no new file uploaded
                    var existing = await _context.PersonalInfos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == model.Id);
                    if (existing != null && string.IsNullOrEmpty(model.PhotoPath))
                    {
                        model.PhotoPath = existing.PhotoPath;
                    }
                }

                _context.PersonalInfos.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(model);
        }

        // ============ EDIT PROJECT (GET) ============
        public async Task<IActionResult> EditProject(int? id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            if (id == null || id == 0)
                return View(new Project()); // Create new project

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            return View(project);
        }

        // ============ EDIT PROJECT (POST) ============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(Project project)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                if (project.Id == 0)
                    _context.Projects.Add(project);
                else
                    _context.Projects.Update(project);

                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(project);
        }

        // ============ DELETE PROJECT ============
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        // ============ EDIT EDUCATION (GET) ============
        public async Task<IActionResult> EditEducation(int? id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            if (id == null || id == 0)
                return View(new Education()); // Create new

            var education = await _context.Educations.FindAsync(id);
            if (education == null)
                return NotFound();
            return View(education);
        }

        // ============ EDIT EDUCATION (POST) ============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEducation(Education education)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                if (education.Id == 0)
                    _context.Educations.Add(education);
                else
                    _context.Educations.Update(education);

                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(education);
        }

        // ============ DELETE EDUCATION ============
        public async Task<IActionResult> DeleteEducation(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            var education = await _context.Educations.FindAsync(id);
            if (education != null)
            {
                _context.Educations.Remove(education);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        // ============ EDIT SKILL (GET) ============
        public async Task<IActionResult> EditSkill(int? id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            if (id == null || id == 0)
                return View(new Skill());

            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound();
            return View(skill);
        }

        // ============ EDIT SKILL (POST) ============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSkill(Skill skill)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                if (skill.Id == 0)
                    _context.Skills.Add(skill);
                else
                    _context.Skills.Update(skill);

                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(skill);
        }

        // ============ DELETE SKILL ============
        public async Task<IActionResult> DeleteSkill(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }
    }
}
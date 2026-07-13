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
                Educations = await _context.Educations.OrderBy(e => e.DisplayOrder).ToListAsync(),
                Skills = await _context.Skills.OrderBy(s => s.DisplayOrder).ToListAsync()
            };
            return View(model);
        }

        // ============ EDIT PERSONAL INFO (GET) ============
        public async Task<IActionResult> EditPersonalInfo()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            var info = await _context.PersonalInfos.FirstOrDefaultAsync();

            // If no record exists, create one with default values
            if (info == null)
            {
                info = new PersonalInfo
                {
                    FullName = "Inkul Azla Hasan",
                    Title = "COMPUTER SCIENCE & ENGINEERING STUDENT",
                    SubTitle = "AI & CYBER SECURITY ENTHUSIAST",
                    Email = "inkulazla@gmail.com",
                    Phone = "+880 1521 514217",
                    Address = "Mirpur-2, Dhaka",
                    Hometown = "Pabna, Rajshahi",
                    LinkedIn = "inkul-azla-hasan-81889b325",
                    Github = "Inkulazlalasan",
                    CareerObjective = "Motivated Computer Science student with a strong interest in software development, web technologies, data analysis and Cyber Security. Eager to apply academic knowledge in real-world projects and continue improving technical and problem-solving skills through hands-on experience.",
                    ProfileSummary = "Final-year B.Sc. CSE student at Bangladesh University of Business and Technology (BUBT), Dhaka; CGPA 3.62/4.00. Proficient in Python, Java, C, C++, C#, and modern web/mobile technologies including React, Node.js, Django, Flask, and REST API development. Built and delivered full-stack projects spanning e-commerce platforms, Android applications, desktop systems, and AI/CV research. Research interest in Artificial Intelligence, Computer Vision, Medical Image Analysis, and Cyber Security. Team-oriented with strong problem-solving and communication skills; consistently high academic performance including perfect GPA in SSC and HSC.",
                    ResearchInterests = "Artificial Intelligence, Computer Vision, Medical Image Analysis, Cyber Security & Digital Forensics, Deep Learning",
                    PhotoPath = "/images/default.jpg"
                };
                _context.PersonalInfos.Add(info);
                await _context.SaveChangesAsync();
            }

            return View(info);
        }

        // ============ EDIT PERSONAL INFO (POST) ============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPersonalInfo(PersonalInfo model, IFormFile? photoFile)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            // 🔥 CRITICAL FIX: Remove PhotoPath validation error since it's optional
            ModelState.Remove("PhotoPath");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Fetch the existing record
            var existing = await _context.PersonalInfos.OrderBy(p => p.Id).FirstOrDefaultAsync();

            // If no record exists, create a new one
            if (existing == null)
            {
                var newInfo = new PersonalInfo
                {
                    FullName = model.FullName,
                    Title = model.Title,
                    SubTitle = model.SubTitle,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    Hometown = model.Hometown,
                    LinkedIn = model.LinkedIn,
                    Github = model.Github,
                    CareerObjective = model.CareerObjective,
                    ProfileSummary = model.ProfileSummary,
                    ResearchInterests = model.ResearchInterests,
                    PhotoPath = "/images/default.jpg"
                };
                _context.PersonalInfos.Add(newInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }

            // Update ALL text fields
            existing.FullName = model.FullName;
            existing.Title = model.Title;
            existing.SubTitle = model.SubTitle;
            existing.Email = model.Email;
            existing.Phone = model.Phone;
            existing.Address = model.Address;
            existing.Hometown = model.Hometown;
            existing.LinkedIn = model.LinkedIn;
            existing.Github = model.Github;
            existing.CareerObjective = model.CareerObjective;
            existing.ProfileSummary = model.ProfileSummary;
            existing.ResearchInterests = model.ResearchInterests;

            // ONLY update PhotoPath if a new file is actually uploaded
            if (photoFile != null && photoFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images"));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photoFile.CopyToAsync(stream);
                }
                existing.PhotoPath = "/images/" + fileName;
            }
            // If no new file, existing.PhotoPath stays UNCHANGED!

            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        // ============ EDIT PROJECT (GET) ============
        public async Task<IActionResult> EditProject(int? id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            if (id == null || id == 0)
                return View(new Project());

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
                return View(new Education());

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
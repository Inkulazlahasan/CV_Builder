using System.ComponentModel.DataAnnotations;

namespace CV_Builder.Models
{
    public class PersonalInfo
    {
        public int Id { get; set; }

        // --- Header ---
        public string FullName { get; set; } = "Inkul Azla Hasan";
        public string Title { get; set; } = "COMPUTER SCIENCE & ENGINEERING STUDENT";
        public string SubTitle { get; set; } = "AI & CYBER SECURITY ENTHUSIAST";

        // --- Contact ---
        public string Email { get; set; } = "inkulazla@gmail.com";
        public string Phone { get; set; } = "+880 1521 514217";
        public string Address { get; set; } = "Mirpur-2, Dhaka";
        public string Hometown { get; set; } = "Pabna, Rajshahi";
        public string LinkedIn { get; set; } = "inkul-azla-hasan-81889b325";
        public string Github { get; set; } = "Inkulazlalasan";

        // --- Main Content (NEW FIELDS) ---
        public string CareerObjective { get; set; } = "Motivated Computer Science student with a strong interest in software development, web technologies, data analysis and Cyber Security. Eager to apply academic knowledge in real-world projects and continue improving technical and problem-solving skills through hands-on experience.";
        public string ProfileSummary { get; set; } = "Final-year B.Sc. CSE student at Bangladesh University of Business and Technology (BUBT), Dhaka; CGPA 3.62/4.00. Proficient in Python, Java, C, C++, C#, and modern web/mobile technologies including React, Node.js, Django, Flask, and REST API development. Built and delivered full-stack projects spanning e-commerce platforms, Android applications, desktop systems, and AI/CV research. Research interest in Artificial Intelligence, Computer Vision, Medical Image Analysis, and Cyber Security. Team-oriented with strong problem-solving and communication skills; consistently high academic performance including perfect GPA in SSC and HSC.";
        public string ResearchInterests { get; set; } = "Artificial Intelligence, Computer Vision, Medical Image Analysis, Cyber Security & Digital Forensics, Deep Learning";

        // --- Styling ---
        public string PhotoPath { get; set; } = "/images/default.jpg";
        public string PrimaryColor { get; set; } = "#2c3e50";
    }
}
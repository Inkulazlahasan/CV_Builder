using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CV_Builder.Migrations
{
    /// <inheritdoc />
    public partial class AddCVFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CareerObjective",
                table: "PersonalInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResearchInterests",
                table: "PersonalInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "PersonalInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PersonalInfos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CareerObjective", "LinkedIn", "ProfileSummary", "ResearchInterests", "SubTitle", "Title" },
                values: new object[] { "Motivated Computer Science student with a strong interest in software development, web technologies, data analysis and Cyber Security. Eager to apply academic knowledge in real-world projects and continue improving technical and problem-solving skills through hands-on experience.", "inkul-azla-hasan-81889b325", "Final-year B.Sc. CSE student at Bangladesh University of Business and Technology (BUBT), Dhaka; CGPA 3.62/4.00. Proficient in Python, Java, C, C++, C#, and modern web/mobile technologies including React, Node.js, Django, Flask, and REST API development. Built and delivered full-stack projects spanning e-commerce platforms, Android applications, desktop systems, and AI/CV research. Research interest in Artificial Intelligence, Computer Vision, Medical Image Analysis, and Cyber Security. Team-oriented with strong problem-solving and communication skills; consistently high academic performance including perfect GPA in SSC and HSC.", "Artificial Intelligence, Computer Vision, Medical Image Analysis, Cyber Security & Digital Forensics, Deep Learning", "AI & CYBER SECURITY ENTHUSIAST", "COMPUTER SCIENCE & ENGINEERING STUDENT" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CareerObjective",
                table: "PersonalInfos");

            migrationBuilder.DropColumn(
                name: "ResearchInterests",
                table: "PersonalInfos");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "PersonalInfos");

            migrationBuilder.UpdateData(
                table: "PersonalInfos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LinkedIn", "ProfileSummary", "Title" },
                values: new object[] { "inkul-azla-hasan", "Final-year B.Sc. CSE student...", "CSE Student & AI Enthusiast" });
        }
    }
}

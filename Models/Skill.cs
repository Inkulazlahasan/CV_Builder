namespace CV_Builder.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Proficiency { get; set; }
        public int DisplayOrder { get; set; } = 0; // NEW
    }
}
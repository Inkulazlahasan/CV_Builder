namespace CV_Builder.Models
{
    public class Education
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string Result { get; set; }
        public string PassingYear { get; set; }
        public int DisplayOrder { get; set; } = 0; // NEW
    }
}
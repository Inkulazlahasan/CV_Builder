namespace CV_Builder.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Nuclei Segmentation";
        public string ShortDescription { get; set; } = "Final-year AI-based biomedical...";
        public string Technologies { get; set; } = "Python, OpenCV, TensorFlow";
        public string GithubLink { get; set; } = "github.com/Inkulazlalasan";
        public int DisplayOrder { get; set; } = 1; // To reorder projects
    }
}
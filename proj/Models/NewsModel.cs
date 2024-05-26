using Microsoft.Build.Framework;
using proj.Logic;

namespace proj.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string HeadLine { get; set; }
        [Required]
        public string Photo { get; set; }
        [Required]
        public string Text { get; set; }

    }
}

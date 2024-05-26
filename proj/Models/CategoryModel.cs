using System.ComponentModel.DataAnnotations;

namespace proj.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Numele categoriei este obligatoriu")]
        public string CategoryName { get; set; }
        public virtual ICollection<ArticleModel> Articles { get; set; }

    }
}

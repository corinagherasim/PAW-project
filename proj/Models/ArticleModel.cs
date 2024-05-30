using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace proj.Models
{

    public class ArticleModel
    {
        public ArticleModel()
        {
            Comments = new List<CommentModel>(); // Inițializează colecția de comentarii
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Continutul articolului este obligatoriu")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "HeadLine-ul este obligatoriu este obligatorie")]
        public string HeadLine { get; set; }
        public string Photo { get; set; }
        [Required(ErrorMessage = "Categoria este obligatorie")]
        public int CategoryId { get; set; }
        public virtual CategoryModel Category { get; set; }
        [BindNever]
        public virtual ICollection<CommentModel> Comments { get; set; }

        public int GetNumberOfComments()
        {
            return Comments.Count;
        }
    }
}

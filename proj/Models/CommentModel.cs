using System.ComponentModel.DataAnnotations;

namespace proj.Models

{
    public class CommentModel
    {
        [Key]
        public int CommentId { get; set; }
        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int ArticleId { get; set; }
        public virtual ArticleModel Article { get; set; }
        public string UserName { get; set; }

    }
}

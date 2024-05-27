using System;
using System.ComponentModel.DataAnnotations;

namespace proj.Models
{
    public class UserSuggestionModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul sugestiei este obligatoriu")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Continutul sugestiei este obligatoriu")]
        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}

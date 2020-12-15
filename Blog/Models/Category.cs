using System.ComponentModel.DataAnnotations;


namespace Blog.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Не коректная информация")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        public string Name { get; set; }
    }
}

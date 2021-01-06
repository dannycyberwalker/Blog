using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Не коректная информация")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 150 символов")]
        public string Headline { get; set; }

        [Display(Name = "Основной текст")]
        [Required(ErrorMessage = "Не коректная информация")]
        [StringLength(10000, MinimumLength = 50, ErrorMessage = "Длина строки должна быть от 50 до 1000 символов")]
        public string Text { get; set; }

        [Display(Name = "Краткое описание")]
        [Required(ErrorMessage = "Не коректная информация")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Длина строки должна быть от 5 до 70 символов")]
        public string ShortDescription { get; set; }

        public User Author { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Раздел статьи")]
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public List<Comment> Comments { get; set; }
        public DateTime CreateTime { get; set; }

        [Display(Name = "Ссылка на картинку")]
        [Required(ErrorMessage = "Не коректная информация")]
        [Url(ErrorMessage = "Не корректная ссылка")]
        public string PictureName { get; set; }   

        public Article()
        {
            CreateTime = DateTime.UtcNow;
        }
    }
}

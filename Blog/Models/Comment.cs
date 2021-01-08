

using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Comment : ICreateTime
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не коректная информация")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 300 символов")]
        public string Text { get; set; }
        public System.DateTime CreateTime { get; set; }

        public User Author { get; set; }
        public string UserId { get; set; }

        public Article Article { get; set; }
        public int ArticleId { get; set; }
        public Comment()
        {
            CreateTime = System.DateTime.UtcNow;
        }
    }
}

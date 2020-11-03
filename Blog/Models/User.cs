using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class User
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 30 символов")]
        [Required(ErrorMessage = "Не коректная информация")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 30 символов")]
        [Required(ErrorMessage = "Не коректная информация")]
        public string LastName { get; set; }

        [Display(Name = "Отображаемое имя(nickname)")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 30 символов")]
        [Required(ErrorMessage = "Не коректная информация")]
        public string NickName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Не верный формат почты")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 до 50 символов")]
        [Required(ErrorMessage = "Не коректная информация")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Длина строки должна быть от 8 до 30 символов")]
        [Required(ErrorMessage = "Не коректная информация")]
        public string Password { get; set; }

        [Display(Name = "Телефон")]
        [Phone(ErrorMessage ="Не верный формат телефона")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 до 15 символов")]
        [Required(ErrorMessage = "Не коректная информация")]
        public string Phone { get; set; }
        public List<Article> Articles { get; set; }
        public DateTime CreateAccountTime { get; set; }
        public User()
        {
            CreateAccountTime = DateTime.Now;
        }
    }
}

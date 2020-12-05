

namespace Blog.Models.ViewModels
{
    public class CreateUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public System.DateTime CreateAccountTime { get; set; }
    }
}

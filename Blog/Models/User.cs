using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public List<Article> Articles { get; set; }
        public DateTime CreateAccountTime { get; set; }
        public User()
        {
            CreateAccountTime = DateTime.Now;
        }
    }
}

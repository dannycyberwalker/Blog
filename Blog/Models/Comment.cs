using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
        public string Author { get; set; }
        public int ArticleId { get; set; }
        public Comment()
        {
            CreateTime = DateTime.UtcNow;
        }
    }
}

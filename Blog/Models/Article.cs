using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Text { get; set; }
        public string ShortDescription { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime CreateTime { get; set; }
        public string PictureLink { get; set; }   
        public int UserId { get; set; }
        public Article()
        {
            CreateTime = DateTime.UtcNow;
        }
    }
}

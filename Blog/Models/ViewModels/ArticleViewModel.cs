using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
    public class ArticleViewModel
    {
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }
        public Article Article { get; set; }
    }
}

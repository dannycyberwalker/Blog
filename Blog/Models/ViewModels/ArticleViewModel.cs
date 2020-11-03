using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
    //ViewModel отдельной статьи
    public class ArticleViewModel
    {
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }
        public Article Article { get; set; }
    }
}

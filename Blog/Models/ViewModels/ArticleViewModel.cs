using System.Collections.Generic;

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

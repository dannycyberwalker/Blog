using System.Collections.Generic;

namespace Blog.Models.ViewModels
{
    public class ArticlesIndexViewModel
    {
        public List<Article> Articles { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public List<Category> Categories { get; set; }
    }
}


namespace Blog.Models.ViewModels
{
    public class ArticlesIndexViewModel
    {
        public System.Collections.Generic.List<Article> Articles { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}

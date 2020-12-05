

namespace Blog.Models.ViewModels
{
    public class ArticleListViewModel
    {
        public System.Collections.Generic.List<ListViewModel> ListViewModels { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}

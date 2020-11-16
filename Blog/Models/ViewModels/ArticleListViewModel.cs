using System.Collections.Generic;

namespace Blog.Models.ViewModels
{
    public class ArticleListViewModel
    {
        public List<ListViewModel> ListViewModels { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}

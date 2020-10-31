using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
    public class ArticleListViewModel
    {
        public List<ListViewModel> ListViewModels { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}

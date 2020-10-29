using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
    public class ListViewModel
    {
        public int CountComments { get; set; }

        public Article Article { get; set; }
    }
}

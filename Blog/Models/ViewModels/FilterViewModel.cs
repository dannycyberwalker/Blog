using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
    public class FilterViewModel
    {
        public string SelectedName { get; set; }
        public FilterViewModel(string name )
        {
            SelectedName = name;
        }
    }
}

using System.Collections.Generic;

namespace Blog.Models.ViewModels
{
    public class ChartViewModel<TypeX>
    {
        public string Title { get; set; }
        public List<TypeX> XValues { get; set; } = new List<TypeX>();
        public List<int> YValues { get; set; } = new List<int>();
    }
}


namespace Blog.Models.ViewModels
{
    public class FilterViewModel
    {
        public string SelectedName { get; set; }
        public Category SelectedCategory { get; set; }
        public FilterViewModel(string name )
        {
            SelectedName = name;
        }
        public FilterViewModel(string name, Category category)
        {
            SelectedName = name;
            SelectedCategory = category;
        }
    }
}

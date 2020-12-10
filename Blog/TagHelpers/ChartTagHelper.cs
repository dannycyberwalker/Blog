using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace Blog.TagHelpers
{
    public class ChartTagHelper : TagHelper
    {
        public ChartViewModel ChartViewModel { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder chart = new StringBuilder();
        }
    }
}

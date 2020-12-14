using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Blog.TagHelpers
{
    public class PlotlyChartTagHelper : TagHelper
    {
        public IEnumerable<object> XValues { get; set; }
        public IEnumerable<int> YValues { get; set; }
        public string Title { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (XValues.Count() != YValues.Count())
                throw new ArgumentException("XValues and YValues contain different number of elements");
            output.Content.SetHtmlContent($"<div id ='{Title}'></div>");
            output.Content.AppendHtml($"<script>Draw('{Title}', {JsonSerializer.Serialize(XValues)} , {JsonSerializer.Serialize(YValues)});</script>");
        }
    }
}

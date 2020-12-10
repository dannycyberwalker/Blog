using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
    public class ChartViewModel
    {
        public string LineName { get; set; }
        public string XName { get; set; }
        public string YName { get; set; }
        /// <summary>
        /// Property with JSON array
        /// </summary>
        public string JSONArray { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHG.Core.ClassData
{
    public class MenuLinks
    {
        public string id { get; set; }
        public string title { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string ImageUrl { get; set; }
        public List<MenuLinks> listChild { get; set; }
        public string url { get; set; }
    }
}

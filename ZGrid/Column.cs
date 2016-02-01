using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZGrid
{
    public class Column
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string ViewTemplate { get; set; }
        public string EditTemplate { get; set; }
        public string HtmlHeader => Title ?? Name;
        public bool Hidden { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
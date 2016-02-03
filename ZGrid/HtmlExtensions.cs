using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ZGrid
{
    public static class HtmlExtensions
    {
        public static Grid<T> GridFor<T>(this HtmlHelper html)
        {
            return new Grid<T>(html);
        }
    }
}
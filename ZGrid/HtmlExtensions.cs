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

        public static MvcHtmlString Test(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(helper.Raw("<p>DENEME DENEME</p>").ToHtmlString());

            return new MvcHtmlString(sb.ToString());
        }
    }
}
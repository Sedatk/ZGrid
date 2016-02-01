using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ZGrid.DataSource
{
    public class AjaxSource : DataSource
    {
        private readonly UrlHelper _urlHelper;
        public string ReadUrl { get; private set; }
        public string CreateUrl { get; private set; }
        public string UpdateUrl { get; private set; }
        public int PageLength { get; private set; } = 3;
        public AjaxSource(UrlHelper urlHelper)
        {
            this._urlHelper = urlHelper;
        }
        public AjaxSource Read(Func<UrlHelper, string> url)
        {
            ReadUrl = url(_urlHelper);
            return this;
        }
        public AjaxSource SetPageLength(int length)
        {
            PageLength = length;
            return this;
        }
        public AjaxSource Create(Func<UrlHelper, string> url)
        {
            CreateUrl = url(_urlHelper);
            return this;
        }
        public AjaxSource Update(Func<UrlHelper, string> url)
        {
            UpdateUrl = url(_urlHelper);
            return this;
        }
    }
}
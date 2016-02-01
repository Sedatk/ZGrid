using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZGrid.DataSource;

namespace ZGrid
{
    public class DatasourceManager
    {
        private readonly UrlHelper _urlHelper;
        public AjaxSource AjaxSource { get; private set; }
        public DatasourceManager(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public AjaxSource Ajax()
        {
            return AjaxSource= new AjaxSource(_urlHelper);
        }
    }
}
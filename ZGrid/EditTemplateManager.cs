using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ZGrid
{
    public class EditTemplateManager
    {
        private readonly string _propertyName;
        public EditTemplateManager(string propertyName)
        {
            _propertyName = propertyName;
        } 
        public string Template { get; private set; }

        public void DropDownList(IDictionary<string,string> items)
        {
            var sb=new StringBuilder();
            sb.AppendLine($@"<select class=""form-control"" value=""{{model.{_propertyName}}}"">");

            foreach (var item in items)
            {
                sb.AppendLine($@"<option value=""{item.Key}"">{item.Value}</option>");
            }

            sb.AppendLine(@"</select>");
            Template = sb.ToString();
        }
    }
}
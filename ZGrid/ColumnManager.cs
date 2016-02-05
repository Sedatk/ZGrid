using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ZGrid
{
    public abstract class ColumnManager
    {
        protected readonly Column Column;
        protected ColumnManager(Column column)
        {
            this.Column = column;
        }

        public abstract void CreateDefaultEditTemplate();

        public ColumnManager Title(string title)
        {
            Column.Title = title;
            return this;
        }
        public ColumnManager Type(string type)
        {
            Column.Type = type;
            return this;
        }
        public ColumnManager ViewTemplate(string template)
        {
            Column.ViewTemplate = template;
            return this;
        }

        public ColumnManager EditTemplate(string template)
        {
            Column.EditTemplate = template;
            return this;
        }

        public ColumnManager EditTemplate(Action<EditTemplateManager> template)
        {
            var editTemplate = new EditTemplateManager(this.Column.Name);
            template(editTemplate);
            Column.EditTemplate = editTemplate.Template;
            return this;
        }

        public ColumnManager Hidden()
        {
            Column.Hidden = true;
            return this;
        }

        public ColumnManager IsReadOnly()
        {
            Column.IsReadOnly = true;
            return this;
        }
    }

    public class ColumnManager<TProperty>:ColumnManager
    {
        public ColumnManager(Column column)
            :base(column)
        {
        }

        private Type GetTypeIgnoreNullable(Type type)
        {
            if (!type.IsGenericType)
                return type;

            if(type.GetGenericTypeDefinition()!=typeof(Nullable<>))
                return type;

            return type.GetGenericArguments()[0];
        }
        public override void CreateDefaultEditTemplate()
        {
            if(!string.IsNullOrEmpty(Column.EditTemplate))
                return;
            
            var sb=new StringBuilder();
            var type = GetTypeIgnoreNullable(typeof (TProperty));
            if (type==typeof(DateTime))
            {
                Column.EditTemplate=
                    $@"<input data-date-format=""yyyy-mm-dd"" data-provide=""datepicker"" class=""form-control input-small"" value=""{{model.{Column.Name}}}""></input>";
            }
        }
        
    }
}
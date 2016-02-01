using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZGrid
{
    public class ColumnManager<T>
    {
        private Column _column;
        public ColumnManager(Column column)
        {
            this._column = column;
        }
        public ColumnManager<T> Title(string title)
        {
            _column.Title = title;
            return this;
        }
        public ColumnManager<T> Type(string type)
        {
            _column.Type = type;
            return this;
        }
        public ColumnManager<T> ViewTemplate(string template)
        {
            _column.ViewTemplate = template;
            return this;
        }

        public ColumnManager<T> EditTemplate(string template)
        {
            _column.EditTemplate = template;
            return this;
        }

        public ColumnManager<T> Hidden()
        {
            _column.Hidden = true;
            return this;
        }

        public ColumnManager<T> IsReadOnly()
        {
            _column.IsReadOnly = true;
            return this;
        } 
    }
}
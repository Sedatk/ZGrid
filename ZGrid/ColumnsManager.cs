using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace ZGrid
{
    public class ColumnsManager<TSource>
    {
        private PropertyInfo GetPropertyInfo<TProperty>(
            Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var type = typeof(TSource);

            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(
                    $"Expression '{propertyLambda.ToString()}' is a method, must be a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(
                    $"Expression '{propertyLambda.ToString()}' is a field, must be property.");
           
            return propInfo;
        }

        public IList<Column> Columns { get; } = new List<Column>();

        public ColumnManager<TSource> For<TProperty>(Expression<Func<TSource, TProperty>> func)
        {
            var propInfo = GetPropertyInfo<TProperty>(func);

            var column = new Column()
            {
                Name = propInfo.Name
            };

            var customAttributes = propInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            var dispAttr = customAttributes.FirstOrDefault();

            var displayAttribute = dispAttr as DisplayAttribute;
            if (displayAttribute != null) column.Title = displayAttribute.Name;
            Columns.Add(column);

            return new ColumnManager<TSource>(column);
        }
    }
}
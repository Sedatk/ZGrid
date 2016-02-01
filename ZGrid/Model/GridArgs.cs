using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ZGrid.Model
{
    public class GridArgs
    {
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }
        [JsonProperty(PropertyName = "start")]
        public int PageStart { get; set; }
        [JsonProperty(PropertyName = "length")]
        public int PageLength { get; set; }
        [JsonProperty(PropertyName = "columns", NullValueHandling = NullValueHandling.Ignore)]
        public ColumnModel[] Columns { get; set; }
        [JsonProperty(PropertyName = "order", NullValueHandling = NullValueHandling.Ignore)]
        public ColumnOrder[] ColumnsOrder { get; set; }

        [JsonIgnore]
        public string OrderbyQuery
        {
            get
            {
                var ret=ColumnsOrder?.Select(p => $"{Columns[p.ColumnIndex].Data} {p.SortingDirection}");
                return ret != null ? string.Join(",", ret) : null;
            }
        }
    }
}
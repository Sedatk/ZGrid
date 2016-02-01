using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ZGrid.Model
{
    public class ColumnOrder
    {
        [JsonProperty(PropertyName = "column")]
        public int ColumnIndex { get; set; }
        [JsonProperty(PropertyName = "dir")]
        public string SortingDirection { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZGrid.Model
{
    public struct ColumnModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "visible", NullValueHandling = NullValueHandling.Ignore)]
        public bool Visible { get; set; }
        [JsonProperty(PropertyName = "render",NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Render { get; set; }

        [JsonProperty(PropertyName = "searchable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Searchable { get; set; }
        [JsonProperty(PropertyName = "orderable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Orderable { get; set; }
    }
}
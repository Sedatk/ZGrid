using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ZGrid.Model.Localization
{
    public class LocalizationPaginator
    {
        private LocalizationPaginator()
        {
            
        }
        [JsonProperty(PropertyName = "previous")]
        public string Previous { get; private set; }

        [JsonProperty(PropertyName = "next")]
        public string Next { get; private set; }

        [JsonProperty(PropertyName = "first")]
        public string First { get; private set; }

        [JsonProperty(PropertyName = "last")]
        public string Last { get; private set; }
        
        public static LocalizationPaginator Factory(bool useIcons=false)
        {
            return new LocalizationPaginator()
            {
                Previous = useIcons ? "<" : Resources.Strings.Previous,
                Next = useIcons ? ">" : Resources.Strings.Next,
                First = useIcons ? "<<" : Resources.Strings.First,
                Last = useIcons ? ">>" : Resources.Strings.Last
            };
        } 
    }
}
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
        
        public static LocalizationPaginator Factory(string language,bool useIcons=false)
        {
            switch (language)
            {
                case "tr-TR":
                    return new LocalizationPaginator()
                    {
                        Previous = useIcons ? "<" : "Önceki sayfa",
                        Next = useIcons ? ">" : "Sonraki sayfa",
                        First = useIcons?"<<":"İlk",
                        Last = useIcons ? ">>" : "Son"
                    };
                default:
                    return new LocalizationPaginator()
                    {
                        Previous = useIcons ? "<" : "Previous",
                        Next = useIcons ? ">" : "Next",
                        First = useIcons ? "<<" : "First",
                        Last = useIcons ? ">>" : "Last"
                    };
            }
        } 
    }
}
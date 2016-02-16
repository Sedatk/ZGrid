using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ZGrid.Model.Localization
{
    public class LocalizationGrid
    {
        private LocalizationGrid()
        {
            
        }
        [JsonProperty(PropertyName = "lengthMenu")]
        public string LengthMenu { get; private set; }
        [JsonProperty(PropertyName = "zeroRecords")]
        public string ZeroRecords { get; private set; }
        [JsonProperty(PropertyName = "info")]
        public string Info { get; private set; }
        [JsonProperty(PropertyName = "infoEmpty")]
        public string InfoEmpty { get; private set; }
        [JsonProperty(PropertyName = "infoFiltered")]
        public string InfoFiltered { get; private set; }
        [JsonProperty(PropertyName = "paginate")]
        public LocalizationPaginator Paginator { get; private set; }
        [JsonProperty(PropertyName = "search")]
        public string Search { get; private set; }//= "Arama"
        public static LocalizationGrid Factory(bool useIcons)
        {
            return new LocalizationGrid()
            {
                LengthMenu = Resources.Strings.LengthMenu,
                ZeroRecords = Resources.Strings.ZeroRecords,
                Info = Resources.Strings.Info,
                InfoEmpty = Resources.Strings.InfoEmpty,
                InfoFiltered = Resources.Strings.InfoFiltered,
                Paginator = LocalizationPaginator.Factory(useIcons),
                Search = Resources.Strings.Search
            };
        } 
    }
}
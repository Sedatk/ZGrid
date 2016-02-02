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
        public static LocalizationGrid Factory(string language,bool useIcons)
        {
            switch (language)
            {
                case "tr-TR":
                    return new LocalizationGrid()
                    {
                        LengthMenu = "Her sayfa için _MENU_ kayıt göster",
                        ZeroRecords = "Kayıt bulunamadı",
                        Info= "_PAGES_ sayfadan _PAGE_. sayfa görüntüleniyor",
                        InfoEmpty = "Herhangi bir kayıt mevcut değil",
                        InfoFiltered = "(Sonuçlar _MAX_ tane kayıttan filtrelendi)",
                        Paginator = LocalizationPaginator.Factory(language,useIcons),
                        Search = "Arama"
                    };
                default:
                    return new LocalizationGrid()
                    {
                        LengthMenu = "Display _MENU_ records per page",
                        ZeroRecords = "Nothing found - sorry",
                        Info = "Showing page _PAGE_ of _PAGES_",
                        InfoEmpty = "No records available",
                        InfoFiltered = "(filtered from _MAX_ total records)",
                        Paginator = LocalizationPaginator.Factory(language,useIcons),
                        Search = "Search"
                    };
            }
        } 
    }
}
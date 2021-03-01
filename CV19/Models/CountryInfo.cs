using System.Collections.Generic;


namespace CV19.Models
{
    //информация о стране
    internal class CountryInfo : PlaceInfo
    {
        public IEnumerable<ProvinceInfo> ProvinceCounts { get; set; }
    }
}

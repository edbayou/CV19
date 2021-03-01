using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CV19.Models
{
    internal class PlaceInfo
    {
        public string Name { get; set; }
        public Point Location { get; set; }
        //информация по количеству подтвержденных случаев
        public IEnumerable<ConfirmedCount> Counts { get; set; }
    }
    //информация о стране
    internal class CountryInfo : PlaceInfo
    {
        public IEnumerable<ProvinceInfo> ProvinceCounts { get; set; }
    }
    //информация о провинциях
    internal class ProvinceInfo : PlaceInfo { }
    
    internal struct ConfirmedCount
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
    
    //когда приходишь в  wpf работаешь со структурами а не сполями
}



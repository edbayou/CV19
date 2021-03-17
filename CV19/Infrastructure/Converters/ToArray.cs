using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
namespace CV19.Infrastructure.Converters
{
    class ToArray : MultiConverter
    {
        //возвращаем значение к которому привязваемся
        public override object Convert(object[] vv, Type t, object p, CultureInfo c)
        {
            //помогает отобразить несколько коллекций и элементов в виде одного списка - CompositeCollection
            var collection = new CompositeCollection();
            foreach (var value in vv)
                collection.Add(value);
            return collection;
        }
        //возвращаем значение в виде массива, пока не поддерживаем
        //public override object[] ConvertBack(object v, Type[] tt, object p, CultureInfo c) => v as object[];
    }
}

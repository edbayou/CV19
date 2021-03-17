using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace CV19.Infrastructure.Converters
{
    internal abstract class MultiConverter : IMultiValueConverter
    {
        //принимаем много бъектов и один тип
        public abstract object Convert(object[] vv, Type t, object p, CultureInfo c);
        // принимаем один объект и множество типов
        public virtual object[] ConvertBack(object v, Type[] tt, object p, CultureInfo c) =>
            throw new NotSupportedException("Обратное преобразование не поддерживается");

    }
}

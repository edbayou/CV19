using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace CV19.Infrastructure.Converters
{
    internal abstract class Converter : MarkupExtension, IValueConverter
    {
        //возвращаем сами себя
        //теперь в том месте где нам нужно встваить конвертер просто пишем Converter={c:Linear 5}
        public override object ProvideValue(IServiceProvider sp) => this;
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException("Обратное преобразование не поддерживается");

    }
}

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace CV19.Infrastructure.Converters
{
    internal class LocationPointToStr : IValueConverter
    {
        //когда привязка узнает что источник изменился и необходимо изменить целевое свойство
        //она берет значение источника  и если есть конвертер она вызыввает метод Convert и передает в object value значение источника
        //и цель конвертера получить новое значение которое как результат установит в привязке свое значение для целевого свойства
        //Type t тип целевого свойства которому выполнена привязка
        //object p передается в привязк через ConverterParameter
        //и культура это текущая культура интерфейса
        public object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (!(value is Point point)) return null;
            
            return $"Lat:{point.X};lon:{point.Y}";
        }
        //если привязка обнаруживает  что изменилось целевое свойство то идет обратное преобразование
        //при этом если эти методы вызывают исключения то они ни как не влияют на работу приложения
        //просто не конверттируют но при этом можно запретить передачу данных в одну или другую сторону
        public object ConvertBack(object value, Type t, object p, CultureInfo c)
        {
            if (!(value is string str)) return null;
            var components = str.Split(';');
            var lat_str = components[0].Split(':')[1];
            var lon_str = components[1].Split(':')[1];
            var lat = double.Parse(lat_str);
            var lon = double.Parse(lon_str);
            return new Point(lat, lon);
            //если ничего не делаем то вбрасываем исключение
            //throw new NotSupportedException();
        }
    }
}

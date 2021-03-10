using CV19.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;

namespace CV19.Services
{
    internal class DataService
    {
        private const string _DataSourceAddress = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();
            //ResponseHeadersRead читаем только заголовки,а  все остальное тело http запроса хранится в сетевой карте
            var response = await client.GetAsync(_DataSourceAddress, HttpCompletionOption.ResponseHeadersRead);
            //потом мы возвращаем поток который нам обеспечит доступ к данным(процесс чтения данных из сетевой карты)

            //ReadAsStreamAsync выводит в виде потока данных Stream
            return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }
        //разбиваем данные на строки
        private static IEnumerable<string> GetDataLinse()
        {
            //для этого получаем поток
            //словили дедлок ).Result т.к. пытаемся работать с асинхронным методом вместе с не асинхронным
            using var data_Stream = GetDataStream().Result;//сдесь мы отправляем запрос на сервер вызывая наш метод и http клиент скачает только заголовок ответа,причем тело зависнит либо в сетевой карте либо просто прекратится передача данных, но сам метод вернет на поток из которого мы можем чиать данные побайтно,т.е data_Stream это захват нашего стрима
            //на его основе создать объект который будет читать строковые данные
            using var data_reader = new StreamReader(data_Stream);//задаем этому объекту паток
            //читаем данные пока не встретится конец потока
            while (!data_reader.EndOfStream)
            {
                //мы считываем одну строку
                var line = data_reader.ReadLine();
                //проверка на пустую строку,если пуста делаем следующий цикл
                if (string.IsNullOrWhiteSpace(line)) continue;
                //делаем наш метод генератором
                //и возвращаем одну строку как результат
                //"Korea, north" преобразуем "Korea north" 
                yield return line.Replace("Korea,", "Kores -");//если бы завести переменную типа массив туда извлек все строки а потом вернул этот массив,это бы заняло место в памяти и если файл будет к примеру 2гигабайта то мы рескуем забиь всю апперативную память
                //в .netfreamwork массивы не могут быть больше 2Gb
            }
        }
        //позваляет получить даты
        private static DateTime[] GetDates() => GetDataLinse()
           .First()
           .Split(",")
           .Skip(4)
           .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
           .ToArray();
        //получем данные по каждой стране
        private static IEnumerable<(string Province, string Country,(double Lat, double Lon) Place, int[] Counts)> GetCountriesData()
        {
            //извлекаем общие данные,перечисление всех строк из файла
            var lines = GetDataLinse()
                .Skip(1) //пропускаем 1 строку
                .Select(line => line.Split(','));//каждую строку разбиваем по разделителю ",", получаем что каждый элемент это ячейка таблицы
                                                 //преобразуем в нужный кортеж
            foreach (var row in lines)
            {
                //выделяем данные и потом груперуем в кортеж, метод трим будет обрезать лишнее
                var province = row[0].Trim();
                var country_name = row[1].Trim(' ', '"');
                var latitude = double.Parse(row[2]);
                var longitude = double.Parse(row[3]);
                //первые 4 штуки пропускаем(широта долгота название провинции)
                //.Select(int.Parse)//каждый из элементов превращаем в целоее число
                var counts = row.Skip(5).Select(int.Parse).ToArray();
                //возвращаем все данные в виде кортежа
                yield return (province, country_name, (latitude, longitude), counts);
            }
        }
        public IEnumerable<CountryInfo> GetData()
        {

            var dates = GetDates();
            var data = GetCountriesData().GroupBy(d => d.Country);
            foreach ( var country_info in data)
            {

                var country = new CountryInfo()
                {
                    Name = country_info.Key,
                    ProvinceCounts = country_info.Select(c => new ProvinceInfo
                    {
                        Name = c.Province,
                        Location = new Point(c.Place.Lat, c.Place.Lon),
                        Counts = dates.Zip(c.Counts, (date, count) => new ConfirmedCount { Date = date, Count = count })
                    })
                };

                yield return country;
            }
            //var russia_data = GetData().First(v => v.Country.Equals("Russia", StringComparison.OrdinalIgnoreCase));
            //затем получаем дату
            //метод Zip обьеденяе последовательнос из первого элемента с последовательностью из второго, т.е.  GetDates -> (date, count) <-ussia_data.Counts
           // Console.WriteLine(string.Join("\r\n", GetDates().Zip(russia_data.Counts, (date, count) => $"{date:dd:MM} - {count}")));
        }
    }
}

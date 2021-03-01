using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CV19Console
{
    class Program
    {
        //источник данных
        private const string data_url = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        // контроль над скачиванием, чтобы не засорять память
        //данный сетод возвращает поток Stream из которого мы можем читать данные
        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();
            //ResponseHeadersRead читаем только заголовки,а  все остальное тело http запроса хранится в сетевой карте
            var response = await client.GetAsync(data_url, HttpCompletionOption.ResponseHeadersRead);
            //потом мы возвращаем поток который нам обеспечит доступ к данным(процесс чтения данных из сетевой карты)
            return await response.Content.ReadAsStreamAsync();
        }
        //читаем текстовые данные
        //разбивка на строки
        //в месте вызова GetDataLinse мы можем в любой момент прервать чтение, при этом весь оставшийся не прочитанный хвост к нам в памеять не попадет и процесс скачивания прервется
        //интерфейс используем чтобы в последствее использовать Linq
        private static IEnumerable<string> GetDataLinse()
        {
            //для этого получаем поток
            using var data_Stream =  GetDataStream().Result;//сдесь мы отправляем запрос на сервер вызывая наш метод и http клиент скачает только заголовок ответа,причем тело зависнит либо в сетевой карте либо просто прекратится передача данных, но сам метод вернет на поток из которого мы можем чиать данные побайтно,т.е data_Stream это захват нашего стрима
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
                yield return line.Replace("Korea,","Kores -");//если бы завести переменную типа массив туда извлек все строки а потом вернул этот массив,это бы заняло место в памяти и если файл будет к примеру 2гигабайта то мы рескуем забиь всю апперативную память
                //в .netfreamwork массивы не могут быть больше 2Gb
            }
        }
        //старая версия yield не может быть асинхронным
        //private static async IEnumerable<string> GetDataLinse()
        //{
        //    //для этого получаем поток
        //    await using var data_Stream = await GetDataStream();
        //    //на его основе создать объект который будет читать строковые данные
        //    using var data_reader = new StreamReader(data_Stream);//задаем этому объекту паток
        //    //читаем данные пока не встретится конец потока
        //    while (!data_reader.EndOfStream)
        //    {
        //        var line = await data_reader.ReadLineAsync();
        //        //проверка на пустую строку,если пуста делаем следующий цикл
        //        if (string.IsNullOrWhiteSpace(line)) continue;
        //        //делаем наш метод генератором
        //        yield return line;
        //    }
        //}
        //метод обработки строк
        //получаем даты
        //получим перечисление строк всего запроса
        //Split(' , ') //получаем массив строк который содержит заголовки каждой калонки файла
        //    .Skip(4) //первые 4 колонки нас не интересуют
        //    .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture)//оставшиеся строки преобразуем в дата тайм
        private static DateTime[] GetDates() => GetDataLinse()
            .First()
            .Split(",")
            .Skip(4)
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
            .ToArray();
        //получаем данные по зараженным по каждой стране
        //применяя этот интерфейс мы можем брать к примеру 10 стран а остальные не будут загружены в память
        //используем картежи, они позваляют в нужном месте определеть структуру данных с нужным набором свойств
        //кортеж отличается от анонимного класса тем что это структура она создается на стеке вызова и не требует работы сброшика мусора
        private static IEnumerable<(string Country, string Province, int[] Counts)> GetData()
        {
            //извлекаем общие данные,перечисление всех строк из файла
            var lines = GetDataLinse()
                .Skip(1) //пропускаем 1 строку
                .Select(line => line.Split(','));//каждую строку разбиваем по разделителю ",", получаем что каждый элемент это ячейка таблицы
                 //преобразуем в нужный кортеж
            foreach(var row in lines)
            {
                //выделяем данные и потом груперуем в кортеж, метод трим будет обрезать лишнее
                var province = row[0].Trim();
                var country_name = row[1].Trim(' ', '"');
                //первые 4 штуки пропускаем(широта долгота название провинции)
                //.Select(int.Parse)//каждый из элементов превращаем в целоее число
                var counts = row.Skip(5).Select(int.Parse).ToArray();
                //возвращаем все данные в виде кортежа
                yield return (country_name, province, counts);
            }
        }
        static void Main(string[] args)
        {
            //старая версия WebClient
            //var client = new HttpClient();
            //получаем ответ от сервера
            //var response = client.GetAsync(data_url).Result;//в конце пишем результ чтбы не делать весь метод асинхронным
            //читаем данные
            //var csv_str = response.Content.ReadAsStringAsync().Result;
            //ИСпользеуем наш метод-генератор
            //foreach (var data_line in GetDataLinse())
            //    Console.WriteLine(string.Join("\r\n", data_line));
            //получаем даты
            //var dates = GetDates();
            //Console.WriteLine(string.Join("\r\n", dates));
            //работаем с кортежем данных
            //сначала фильтруем по стране
            var russia_data = GetData().First(v => v.Country.Equals("Russia", StringComparison.OrdinalIgnoreCase));
            //затем получаем дату
            //метод Zip обьеденяе последовательнос из первого элемента с последовательностью из второго, т.е.  GetDates -> (date, count) <-ussia_data.Counts
            Console.WriteLine(string.Join("\r\n", GetDates().Zip(russia_data.Counts, (date, count) => $"{date:dd:MM} - {count}")));
            Console.ReadLine();
        }
    }
}

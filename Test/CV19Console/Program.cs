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
                yield return line;//если бы завести переменную типа массив туда извлек все строки а потом вернул этот массив,это бы заняло место в памяти и если файл будет к примеру 2гигабайта то мы рескуем забиь всю апперативную память
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
            //    Console.WriteLine(data_line);
            //получаем даты
            var dates = GetDates();
            Console.WriteLine(string.Join("\r\n", dates));
            Console.ReadLine();
        }
    }
}

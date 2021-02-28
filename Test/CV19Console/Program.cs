using System;
using System.Net.Http;

namespace CV19Console
{
    class Program
    {
        //источник данных
        const string data_url = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

        static void Main(string[] args)
        {
            //старая версия WebClient
            var client = new HttpClient();
            //получаем ответ от сервера
            var response = client.GetAsync(data_url).Result;//в конце пишем результ чтбы не делать весь метод асинхронным
            //читаем данные
            var csv_str = response.Content.ReadAsStringAsync().Result;
            Console.ReadLine();
        }
    }
}

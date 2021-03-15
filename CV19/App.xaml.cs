using CV19.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CV19
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //добавляем свойство чтобы определяло в дезайнере VS мы работаем или запускаем .exe
        public static bool IsDesignMode { get; private set; } = true;
        protected override void OnStartup(StartupEventArgs e)
        {
            IsDesignMode = false;
            base.OnStartup(e);
            /*//на самом деле p:Freeze="true"/ работает так
            var brush = new SolidColorBrush(Colors.White);
            brush.Freeze();
            //и теперь нельзя поменять свойства у этого обьекта brush.Color = Colors.Black; но можно кланировать
            brush.Clone().Color = Colors.Black;*/
        }
        /* //для тестироваем подкидывае наши методы прям в запуск программы и с помощью точки остонова смотрим что получилось
         protected override void OnStartup(StartupEventArgs e)
         {
             IsDesignMode = false;
             base.OnStartup(e);
             //вызываем метод сразу в приложении так делать нельзя
             var service_test = new DataService();
             var countries = service_test.GetData().ToArray();
         }*/

    }
}

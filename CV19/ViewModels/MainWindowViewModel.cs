using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.ViewModels.Base;
namespace CV19.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        /*
        //при желании можем переопределить Dispose т.к. наследуемся от  ViewModel и он виртуальный
        protected override void Dispose(bool Disposing)
        {
            base.Dispose(Disposing);
        }
        */
        #region Тестовый набор данных для визуализации графиков
        private IEnumerable<DataPoint> _TestDataPoints;
        public IEnumerable<DataPoint> TestDataPoints { get => _TestDataPoints; set => Set(ref _TestDataPoints, value); }
        #endregion
        #region Тестовая работа со вкладкой
        /// <summary>
        /// номер выбранной вкладки
        /// </summary>
        private int _SelectedPageIndex;
        public int SelectedPageIndex { get => _SelectedPageIndex; set => Set(ref _SelectedPageIndex, value); }
        #endregion
        #region Заголовок окна
        //создаем свойсво имя окна
        //поле для хранения данных
        private string _Title = "Анализ статистики CV19";//значение по умолчанию
        //само свойство
        /// <summary> Заголовок окна </summary>
        public string Title
        {
            //возвращаем само поле
            get => _Title;
           //еще проще
            set => Set(ref _Title, value);
            //подробнее что происходит
            /*set
            {
           
                //как должно быть
                if (Equals(_Title, value)) return;
                _Title = value;
                OnPropertyChnged();
           
                //так как есть метод Set делаем проще
                Set(ref _Title, value);
            }*/
        }
        #endregion
        #region Статус программы
        /// <summary>Статус программы</summary>
        private string _Status = "Готово!";//значение по умолчанию
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
        #endregion
        //создаем команды
        #region Команды
        #region Тестовая команда управления вкладками
        public ICommand ChangeTebIndexCommand { get; }
        private bool CanChangeTebIndexCommandExecut(object p) => true;//в нашем случае команда всегда доступна

        private void OnChangeTebIndexCommandExecuted(object p)
        {
            if (p is null) return;
            SelectedPageIndex += Convert.ToInt32(p);
        }
        #endregion
        #region Команда закрытия программы
        //команда закрытия программы

        //создаем свойство, делаем приписку к названию Command,чтобы отличать команды от простых свойств
        //свойство от поля отличается наличием {get; set;}
        public ICommand CloseApplicationCommand { get; } //сама команда
        //создаем два метода для нашей команды
        //что она делает
        private bool CanCloseApplicationCommandExecut(object p) => true;//в нашем случае команда всегда доступна
        
        private void OnCloseApplicationCommandExecuted(object p)
        {
            //выполняется когда команда выполняется
           // Application.Current.Shutdown();
        }
        #endregion
        #endregion
        //конструктор для viemodel
        public MainWindowViewModel()
        {
            //создаем объект нашей команды внутри конструктора
            #region Команды
            //задаем значения
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecut);
            //изменение активной вкладки
            ChangeTebIndexCommand = new LambdaCommand(OnChangeTebIndexCommandExecuted, CanChangeTebIndexCommandExecut);
            #endregion
            var data_points = new List<DataPoint>((int)(360 / 0.1));
            for(var x= 0d; x<= 360; x+= 0.1)
            {
                const double to_rad = Math.PI / 100;
                var y = Math.Sin(x * to_rad);
                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }
            TestDataPoints = data_points;
        }
    }
}

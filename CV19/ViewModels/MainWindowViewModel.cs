using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CV19.Infrastructure.Commands;
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
            Application.Current.Shutdown();
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
            #endregion
        }
    }
}

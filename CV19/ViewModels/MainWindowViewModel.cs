using System;
using System.Collections.Generic;
using System.Text;
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
        }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CV19.ViewModels.Base
{ //интерфейс INotifyPropertyChanged уведомляет о том что внутри нашего объекта изменилось какоето свойство
    //тоесть если коктоето свойство объекта изменилось то этот интерфейс может обновить визуальную часть.
    //абстарктный клас можно влючать в другие классы ное ему не нужен конструктор вместо этого используется конструктор класса наследника
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        //PropertyChangedEventHandler - делегат, PropertyChanged - событие
        public event PropertyChangedEventHandler PropertyChanged;
        //атрибут CallerMemberName Позволяет получить имя свойства или метода вызывающего метод объекта.
        //теперь мы можем не указывать в параметрах имя свойства компилятор автаматически подставит вместо переменной
        //PropertyName имя метода из которого вызывается данная процедура
        //метод генерирует событие по переданному свойству
        protected virtual void OnPropertyChnged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        //Set метод, его задача обновлять значение свойства для которого определено поля в котором это свойство 
        //хранит свои данные(задача разрешить кольцевые методы свойств которые могут возникать
        //одно свойство может менять другое  а оно третье, а третье первое и чтобы не зацакливались эти обнавления
        //мы делаем проверку
        //ref T - ссылка на поле свойства, T value -передаем новое значени которое мы ходим установить, последнее - параметр будет определятся самостоятельно компилятором это имя нашего свойства
        //которое мы передаем в void OnPropertyChnged
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            //если поле соответствует тому значению которое мы передали то ничего не делаем
            if (Equals(field, value)) return false;
            //в противном случае если свойство действительно изменилось обновляем поле
            field = value;
            //и генерируем метод OnPropertyChnged
            OnPropertyChnged(PropertyName);
            return true; // по этому флагу мы можем определять выполнилась ли изменение свойства и в последствии выполнить еще какуюто работу по обнавлению других свойств например которые с ним связаны
        }
    }
}

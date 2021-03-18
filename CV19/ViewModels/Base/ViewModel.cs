using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using System.Xaml;

namespace CV19.ViewModels.Base
{ 
    //интерфейс INotifyPropertyChanged уведомляет о том что внутри нашего объекта изменилось какоето свойство
    //тоесть если коктоето свойство объекта изменилось то этот интерфейс может обновить визуальную часть.
    //абстарктный клас можно влючать в другие классы ное ему не нужен конструктор вместо этого используется конструктор класса наследника
    internal abstract class ViewModel : MarkupExtension, INotifyPropertyChanged, IDisposable
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
        public override object ProvideValue(IServiceProvider sp)
        {
            //попробуем из модели достать эелементы представления
            //извлекаем все сервисы которые нам помогут
            //целевой объект которому выполняется обращение 
            var value_target_service = sp.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;//в него передаем тип сервиса который мы хотим
            //корень дерева( наше окно) эти сервисы позваляют получить доступ к этим объектам
            var root_object_service = sp.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            OnInitialized(value_target_service?.TargetObject, value_target_service?.TargetProperty, root_object_service?.RootObject);
            return this;
        }
        //получим наши данные из метода
        //так нельзя потому что будет утечка памяти, создается модель представление
        //она получает сылку на окно и захватывает ее и держит,после этого если например будут работать диалоги
        //окна будут создаваться, закрываться, мы будем терять ссылку, а модель представление будет держать ссылку на окно
        //и в результате GC не сможет удалить это окно
       // private object _Target;
        //private object _Root;
        //поэтому используем не жесткие ссылки а мягкие, онип озволяют ГК удалять объекты но при этом хранит ссылку и получать к нему доступ
        private WeakReference _TargetRef;
        private WeakReference _RootRef;
        //теперь делаем свойства которые позволят получать доступ к объектам через ссылки
        public object TargetObject => _TargetRef.Target;
        public object RootObject => _RootRef.Target;
        protected virtual void OnInitialized(object Target,object Property, object Root)
        {
            _TargetRef = new WeakReference(Target);
            _RootRef = new WeakReference(Root);
        }
        //пример реализации метода Dispose
        /*//если появится деструктор
        ~ViewModel()
        {
            Dispose(false);
        }*/
        public void Dispose()
        {
            Dispose(true);
        }
        private bool _Disposed;
        //метод с параметром  Disposing,он виртуальный чтобы наследники его могли переопределить
        protected virtual void Dispose(bool Disposing)
        {
            if (!Disposing || _Disposed) return;
            _Disposed = true;//освобождать нечего
            //освобождение управляемых ресурсов
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestLibFramework
{
    public abstract class Command : ICommand

    {
        //событее которе генерируется когда CanExecute переходит из одного состояния в другое(правда->лож)
        public event EventHandler CanExecuteChanged
        {
            //можем опредилить как нам хочется а можем передать управлениее системе wpf
            //для этого добовляем два элемента
            add => CommandManager.RequerySuggested += value;//передаем все изменения в CommandManager и он сам все делает
            remove => CommandManager.RequerySuggested -= value;
        }
        //булевская фйнкция если false то команду выполнить нельзя, и элемент к которму привязана команда отключается автоматически
        //если мы хотим вызвать отключение какихто элементов на экране достаточно возвращать лож
        //abstract -не нужно определять их реализацией займется наследник
        public abstract bool CanExecute(object parameter);

        //то что должно быть выполнено самой командой
        public abstract void Execute(object parameter);

    }
}

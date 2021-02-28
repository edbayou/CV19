using CV19.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CV19.Infrastructure.Commands
{
    //RelayСommand
    //также бывает тепизированный RelayСommand где object приводится к нужному типу
    internal class LambdaCommand : Command
    {
        //заводим два приватных поля
        //поля отмеченые readonly работают быстрее
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;
        //в конструкторе мы получаем два делегата один будет выполняться методом CanExecute, второй Execute
        //комманды из разметки могут получать параметры(это может быть что угодно) поэтому  мы делегату Action передаем параметр <object> в последствии мы его 
        //преобразуем к нужному нам виду, сразу приводим второй параметр к null чтобы можно было его не указывать
        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            //задаем наши приватные поля
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }
        //связываем наши методы _CanExecute с CanExecute. _CanExecute? подразумеваем что может быть пустая сылка
        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => _Execute(parameter);
    }
}

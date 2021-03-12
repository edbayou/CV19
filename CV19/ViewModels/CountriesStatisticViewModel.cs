using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModel
    {
        private readonly MainWindowViewModel _MainModel;
        //связваем вюмодели между собой при этом в главной создаем вторичную
        public CountriesStatisticViewModel(MainWindowViewModel MainModel) { _MainModel = MainModel; }
        //теперь мы сможем вызывать методы из основной модели а ана у нас и также передавать/устанавливать значения свойст
    }
}

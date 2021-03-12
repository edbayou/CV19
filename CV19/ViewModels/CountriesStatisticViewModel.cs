using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.Services;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModel
    {
        //сервис который будет извлекать данные
        private DataService _DataService;
        //можно так 
        //private readonly MainWindowViewModel _MainModel;
        //но мы сделаем как свойство чтобы можно было достучаться из представления
        private MainWindowViewModel MainModel { get; }

        #region Свойство Contries : IEnumerable<CountryInfo> - Будет содержать статистику по странам и при этом seter будет приватным 
        ///<summary> Будет содержать статистику по странам и при этом seter будет приватным </summary>
        private IEnumerable<CountryInfo> _Contries;

        public IEnumerable<CountryInfo> Contries
        {
            get => _Contries;
            private set => Set(ref _Contries, value); }
        #endregion

        //прописываем команды 
        #region Команды.
        public ICommand RefreshDataCommand { get; }
        //будет выгружать данные из сервиса
        private void OnRefreshDataCommandExecuted(object p) 
        {
            Contries = _DataService.GetData();
        }
        #endregion
        //связваем вюмодели между собой при этом в главной создаем вторичную
        public CountriesStatisticViewModel(MainWindowViewModel MainModel) 
        { 
           this.MainModel = MainModel;
            _DataService = new DataService();
            RefreshDataCommand = new LambdaCommand(OnRefreshDataCommandExecuted);
        }
        //теперь мы сможем вызывать методы из основной модели а ана у нас и также передавать/устанавливать значения свойст
    }
}

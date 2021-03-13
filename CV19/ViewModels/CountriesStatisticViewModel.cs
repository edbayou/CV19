using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.Services;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModel
    {
        //сервис который будет извлекать данные
        private readonly DataService _DataService;
        //можно так 
        //private readonly MainWindowViewModel _MainModel;
        //но мы сделаем как свойство чтобы можно было достучаться из представления
        private MainWindowViewModel MainModel { get; }

        #region Свойство Countries : IEnumerable<CountryInfo> - Будет содержать статистику по странам и при этом seter будет приватным 
        ///<summary> Будет содержать статистику по странам и при этом seter будет приватным </summary>
        private IEnumerable<CountryInfo> _Countries;

        public IEnumerable<CountryInfo> Countries
        {
            get => _Countries;
            private set => Set(ref _Countries, value); }
        #endregion
        #region SelectedCountry : CountryInfo - Выбранная страна

        /// <summary>Выбранная страна</summary>
        private CountryInfo _SelectedCountry;

        /// <summary>Выбранная страна</summary>
        public CountryInfo SelectedCountry { get => _SelectedCountry; set => Set(ref _SelectedCountry, value); }

        #endregion
        //прописываем команды 

        #region Команды.
        public ICommand RefreshDataCommand { get; }
        //будет выгружать данные из сервиса
        private void OnRefreshDataCommandExecuted(object p) 
        {
            Countries = _DataService.GetData();
        }
        #endregion
        //делаем еще один конструктор  им ожно его использовать в процессе разработки с d: в дизайнере
        //тоесть дизайнер создаст обьект и не только будет посказывать доступные методы а выдовать данные по этом объекту
        /// <summary> отладочный конструктор используемы в процессе разработки в дизайнере /// </summary>
        public CountriesStatisticViewModel() : this(null) 
        {
            //страхуемся чтобы при использовании    ничего не делалась если это не дизайнер
            if (!App.IsDesignMode)
                throw new InvalidOperationException("Вызов конструктора не предназначеного для использования");
            //т.е. можем создать чтото для визуализации в дизайнере например создадим 10 стран
            _Countries = Enumerable.Range(1,10)
                .Select(i => new CountryInfo
                {
                    Name = $"Country {i}",
                    ProvinceCounts = Enumerable.Range(1, 10).Select(j => new PlaceInfo
                    { 
                        Name = $"Province {i}",
                        Location = new Point(i,j),
                        Counts = Enumerable.Range(1, 10).Select(k => new ConfirmedCount
                        {
                            Date = DateTime.Now.Subtract(TimeSpan.FromDays(100 - k)),
                            Count = k
                
                        }).ToArray()
                    }).ToArray()
                }).ToArray();
        }
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

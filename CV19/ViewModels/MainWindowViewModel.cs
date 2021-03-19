using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.Models.Deconat;
using CV19.ViewModels.Base;
using System.Windows.Markup;
using System.Windows;

namespace CV19.ViewModels
{
    [MarkupExtensionReturnType(typeof(MainWindowViewModel))]
    internal class MainWindowViewModel : ViewModel
    {
        //создаем вторичную модель в главной
        //private readonly CountriesStatisticViewModel _CountriesStatistic;
        //превратили в свойство
        public CountriesStatisticViewModel CountriesStatistic { get; }
        /*
        //при желании можем переопределить Dispose т.к. наследуемся от  ViewModel и он виртуальный
        protected override void Dispose(bool Disposing)
        {
            base.Dispose(Disposing);
        }
        */
        //показ разнородных данных
        public object[] CompositeColection { get; }
        ///Свойства



        #region Свойство SelectedCompositeValue - Выбранный непонятный элемент 
        ///<summary> Выбранный непонятный элемент </summary>
        private object _SelectedCompositeValue;

        public object SelectedCompositeValue { get => _SelectedCompositeValue; set => Set(ref _SelectedCompositeValue, value); }
        #endregion
       
        #region создаем студентов
        //коллекция групп
        public ObservableCollection<Group> Groups { get; }
        #endregion
        #region Выбранная группа
        /// <summary>Выбранная группа</summary>
        private Group _SelectedGroup;//значение по умолчанию
        public Group SelectedGroup
        {
            get => _SelectedGroup;
            //set => Set(ref _SelectedGroup, value);
            set
            {
                //если свойства не изменились то ничего не делаем, в противном случае устанавливаем значение нашего объекта
                if (!Set(ref _SelectedGroup, value)) return;
                //подразумеваем что в валуем может оказаться пустая ссылка  value?
                _SelectedGroupStudents.Source = value?.Students;
                //попытались уведометь об обнавлении не сработало
                //_SelectedGroupStudents.View.Refresh();
                //пробуем такой метод,вызываем событие какое еще свойство изменилось
                //уведомлям интерфейс что произошли изменения еще гдето во viewmodele
                OnPropertyChnged(nameof(SelectedGroupStudents));
            }
        }
        #endregion

        #region Свойство StudentFilterText : string - Текст фильтров студентов 
        ///<summary> Текст фильтров студентов </summary>
        private string _StudentFilterText;

        public string StudentFilterText
        {
            get => _StudentFilterText; 
            set
            {
               if(!Set(ref _StudentFilterText, value)) return;
                _SelectedGroupStudents.View.Refresh();
            }
        }
        #endregion

        #region SelectedGroupStudents и Филтрация студентов 

        //источник данных сразу его создадим один раз , а устанавливать значение Source  объекта _SelectedGroupStudents будем в сетере SelectedGroup
        //посути представление списка
        private readonly CollectionViewSource _SelectedGroupStudents = new CollectionViewSource();
        //создаем метод
        private void OnStudentFiltred(object sender, FilterEventArgs e)
        {
           
            //если объект не является студентом то ничего не делаем
            if (!(e.Item is Student student))
            {
                e.Accepted = false;
                return;
            }
            var filter_text = _StudentFilterText;
            //если текст филтра пустой то ничего не делаем
            if (string.IsNullOrWhiteSpace(filter_text))
                return;
            //студента с пустой ссылкой на имя тоже пропускаем иначе логика сломается
            if (student.Name is null || student.Surname is null || student.Patronymic is null)
            {
                e.Accepted = false;
                return;
            }
            //если уже содержится искомый текст то ничего не делаем, ленивая логика
            if (student.Name.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if (student.Surname.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if (student.Patronymic.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            
            //if (group.Description != null && group.Description.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            //если не содержит не в имени ни в описании искомый текст то отбрасываем этот элемент
            e.Accepted = false;
        }
        //свойство будет возвращать тип и View
        public ICollectionView SelectedGroupStudents => _SelectedGroupStudents?.View;

        #endregion
        #region Тестовый набор данных для визуализации графиков
        private IEnumerable<DataPoint> _TestDataPoints;
        public IEnumerable<DataPoint> TestDataPoints { get => _TestDataPoints; set => Set(ref _TestDataPoints, value); }
        #endregion
        #region Тестовая работа со вкладкой
        /// <summary>
        /// номер выбранной вкладки
        /// </summary>
        private int _SelectedPageIndex = 0;
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
        #region Создаем тестовых студентов 
        //в количестве 10000 если у нас запус exe и 10 если рендеринг
        public IEnumerable<Student> TestStudent => Enumerable.Range(1, App.IsDesignMode ? 10 : 10000)
            .Select(i => new Student
            {
                Name = $"Имя {i}",
                Surname = $"Фамилия {i}"
            });
        #endregion
        public DirectoryViewModel DiskRootDir { get; } = new DirectoryViewModel("c:\\");

        #region Свойство SelectedDirectoy : DirectoryViewModel - выбранная директория 
        ///<summary> выбранная директория </summary>
        private DirectoryViewModel _SelectedDirectoy;

        public DirectoryViewModel SelectedDirectoy { get => _SelectedDirectoy; set => Set(ref _SelectedDirectoy, value); }
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
            //теперь можем управлять окном через мягкую ссылку
          (RootObject as Window)?.Close();
        }
        #endregion
        #region Создание новой группы
        public ICommand CreateGroupCommand { get; }
        private bool CanCreateGroupCommandExecut(object p) => true ;
        private void OnCreateGroupCommandExecuted(object p)
        {
            var group_max_index = Groups.Count + 1;
            var new_group = new Group
            {
                Name = $"Группа {group_max_index}",
                Students = new ObservableCollection<Student>()
            };
            Groups.Add(new_group);
         }
        #endregion
        #region Удаление новой группы
        public ICommand DeleteGroupCommand { get; }
        private bool CanDeleteGroupCommandExecut(object p) => p is Group group && Groups.Contains(group);//группу мы можем удалить если параметр является Группой и он существует в списке групп

        private void OnDeleteGroupCommandExecuted(object p)
        {

            if (!(p is Group group)) return;
            //устанавиливаем индекс на предыдушую позицию от удаленной группы
            var group_index = Groups.IndexOf(group);
            Groups.Remove(group);
            if (group_index < Groups.Count)
                SelectedGroup = Groups[group_index];
        }
        #endregion
        #endregion
        //конструктор для viemodel
        public MainWindowViewModel()
        {
            //теперь вьюмодели знают друг о друге передумали
            //_CountriesStatistic = new CountriesStatisticViewModel(this);
            //и сделали так
            CountriesStatistic = new CountriesStatisticViewModel(this);
            //создаем объект нашей команды внутри конструктора
            #region Команды
            //задаем значения
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecut);
            //изменение активной вкладки
            ChangeTebIndexCommand = new LambdaCommand(OnChangeTebIndexCommandExecuted, CanChangeTebIndexCommandExecut);
            //редактирование группы
            CreateGroupCommand = new LambdaCommand(OnCreateGroupCommandExecuted, CanCreateGroupCommandExecut);
            DeleteGroupCommand = new LambdaCommand(OnDeleteGroupCommandExecuted, CanDeleteGroupCommandExecut);
            #endregion
            var data_points = new List<DataPoint>((int)(360 / 0.1));
            for(var x= 0d; x<= 360; x+= 0.1)
            {
                const double to_rad = Math.PI / 100;
                var y = Math.Sin(x * to_rad);
                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }
            TestDataPoints = data_points;
            //определяем создание студентов
            //создаем переменную
            var student_index = 1;
            //создаем колекцию студентов
            var students = Enumerable.Range(1, 10).Select(i => new Student
            {
                Name = $"Name {student_index}",
                Surname = $"Surname {student_index}",
                Patronymic = $"Patronymic {student_index++}",
                Birthday = DateTime.Now,
                Rating = 0
            });
            //создаем пачкой данные чтоб поместить их в колекциюю разом а не по одному например массив или список а потом передать его в конструктор  ObservableCollection<Group>(вот сюда)
           //создаем перечисление в кол-ве 20 шт и возьмем каждое число и на его основе создадим Группу Group
            var groups = Enumerable.Range(1, 20).Select(i => new Group
            {
                Name = $"Группа {i}",
                Students = new ObservableCollection<Student>(students)
            }
            );
            Groups = new ObservableCollection<Group>(groups);
            
            
            var data_list = new List<object>();


            data_list.Add("Привет мир");
            data_list.Add(42);
            var group = Groups[1];

            data_list.Add(group);
            data_list.Add(group.Students.First());
            CompositeColection = data_list.ToArray();
            //добавляем филтр через конструктор
            _SelectedGroupStudents.Filter += OnStudentFiltred;
            //можно сортировать по быванию
            //_SelectedGroupStudents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
            //групировку данных
            //_SelectedGroupStudents.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
        }
    }
}

using CV19.Models.Deconat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace CV19
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //FilterEventArgs e это параметр,в нем есть сам элемент Items который подвергается филтрации на каждом моменте viewSource т.е. каждый раз вызываает это событие
        private void GroupsCollectionFilter(object sender, FilterEventArgs e)
        {


            //работает над колекцией и каждый раз вызывает событие
            //если объект не является группой то ничего не делаем
            if (!(e.Item is Group group)) return;
            //группы с пустой ссылкой на имя тоже пропускаем иначе логика сломается
            if (group.Name is null) return;
            //если текст фильтра отсутствует то пропускаем все элементы
            var filter_text = GroupNameFilterText.Text;
            //если текст филтра отсутствует то мы пропускаем все элементы
            if (filter_text.Length == 0) return;
            //сам фильтр
            //если уже содержится искомый текст то ничего не делаем, ленивая логика
            if (group.Name.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if(group.Description != null && group.Description.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            //если не содержит не в имени ни в описании искомый текст то отбрасываем этот элемент
            e.Accepted = false;
        }
        //нужно отследить событие что текстбокс изменне
        private void OnGroupsFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            //достаем по ресурсу
            var text_box = (TextBox)sender;
            var collection = (CollectionViewSource)text_box.FindResource("GroupsCollection");
            collection.View.Refresh();
        }
    }
}

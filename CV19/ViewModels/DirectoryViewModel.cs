using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using CV19.ViewModels.Base;
using System.Diagnostics;

namespace CV19.ViewModels
{
    //INotifyPropertyChanged если не наследоваться от этого интефейса то ссылка на модель остается в памяти и не освождается даже если визуальная часть на нее не ссылается
    class DirectoryViewModel : ViewModel
    {

        private readonly DirectoryInfo _DirectoryInfo;
        /* public IEnumerable<DirectoryViewModel> SubDirectories => _DirectoryInfo.EnumerateDirectories()
             .Select(dir_info => new DirectoryViewModel(dir_info.FullName));*/
        public IEnumerable<DirectoryViewModel> SubDirectories
        {
            get
            {
                try
                {
                    return _DirectoryInfo.EnumerateDirectories()
                              .Select(dir_info => new DirectoryViewModel(dir_info.FullName));
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return Enumerable.Empty<DirectoryViewModel>();
            }
        }

        //перечисление файлов
        // c проверкой
        public IEnumerable<FileViewModel> Files
        {
            get
            {
                try
                {
                    var files =  _DirectoryInfo.EnumerateFiles()
                                    .Select(file => new FileViewModel(file.FullName));
                    return files;
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return Enumerable.Empty<FileViewModel>();
            }
        }
        //без
        //public IEnumerable<FileViewModel> Files => _DirectoryInfo.EnumerateFiles().Select(file => new FileViewModel(file.FullName));

        //дочерние элементы дериктории, субдеректории
        public IEnumerable<object> DirectoryItems
        {
            get
            {
                try
                {
                    return SubDirectories.Cast<object>().Concat(Files);

                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return Enumerable.Empty<object>();
            }
        }

        public string Name => _DirectoryInfo.Name;
        public string Path => _DirectoryInfo.FullName;
        public DateTime CreationTime => _DirectoryInfo.CreationTime;
        //еди на вести на класс и нажать ctrl+. то привращается из {} в => использовать тело выражения для конструкторов 
        public DirectoryViewModel(string Path) => _DirectoryInfo = new DirectoryInfo(Path);
    }
    class FileViewModel : ViewModel
    {
        private FileInfo _FileInfo;
        public FileViewModel(string Path) => _FileInfo = new FileInfo(Path);
        /* public FileViewModel(string Path)
         {
             _FileInfo = new FileInfo(Path);
         }*/
        public string Name => _FileInfo.Name;
        public string Path => _FileInfo.FullName;
        public DateTime CreationTime => _FileInfo.CreationTime;
    }
}

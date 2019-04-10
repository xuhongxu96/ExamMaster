using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ExamMaster.Wpf.ViewModels
{
    public class FileList : INotifyPropertyChanged
    {
        private string _directoryPath = "";
        public string DirectoryPath
        {
            get => _directoryPath;
            set
            {
                _directoryPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DirectoryPath)));
            }
        }

        public ObservableCollection<string> DocxFiles { get; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

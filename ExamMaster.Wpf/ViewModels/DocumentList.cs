using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ExamMaster.Wpf.ViewModels
{
    public class DocumentList : INotifyPropertyChanged
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

        public ObservableCollection<DocumentModel> Documents { get; } = new ObservableCollection<DocumentModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

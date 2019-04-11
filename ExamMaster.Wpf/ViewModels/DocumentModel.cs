using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ExamMaster.Wpf.ViewModels
{
    public class DocumentModel : INotifyPropertyChanged
    {
        private string _relativePath = "";
        public string RelativePath
        {
            get => _relativePath;
            set
            {
                _relativePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RelativePath)));
            }
        }

        public ObservableCollection<Exception> Exceptions { get; } = new ObservableCollection<Exception>();

        public ObservableCollection<DocumentSection> Sections { get; } = new ObservableCollection<DocumentSection>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

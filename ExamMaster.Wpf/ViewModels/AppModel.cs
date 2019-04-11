using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ExamMaster.Wpf.ViewModels
{
    public class AppModel : INotifyPropertyChanged
    {
        public DocumentList DocumentList { get; } = new DocumentList();

        private DocumentModel _currentModel = new DocumentModel();
        public DocumentModel CurrentDocument
        {
            get => _currentModel;
            set
            {
                _currentModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDocument)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

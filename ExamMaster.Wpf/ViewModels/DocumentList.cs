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

        private ObservableCollection<DocumentModel> _documents;
        public ObservableCollection<DocumentModel> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Documents)));
            }
        }

        private DocumentModel _selectedDocument = new DocumentModel("");
        public DocumentModel SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                _selectedDocument = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDocument)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DocumentList(string directoryPath)
        {
            _directoryPath = directoryPath;
            _documents = new ObservableCollection<DocumentModel>();
        }

        public DocumentList(string directoryPath, IEnumerable<DocumentModel> documents)
        {
            _directoryPath = directoryPath;
            _documents = new ObservableCollection<DocumentModel>(documents);
        }
    }
}

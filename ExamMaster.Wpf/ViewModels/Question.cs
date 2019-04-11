using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ExamMaster.Wpf.ViewModels
{
    public class Question : INotifyPropertyChanged
    {
        private string _header = "";
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Header)));
            }
        }

        private string _body = "";
        public string Body
        {
            get => _body;
            set
            {
                _body = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Body)));
            }
        }

        public ObservableCollection<Question> Questions { get; } = new ObservableCollection<Question>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

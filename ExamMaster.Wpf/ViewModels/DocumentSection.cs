using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ExamMaster.Wpf.ViewModels
{
    public class DocumentSection : INotifyPropertyChanged
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public ObservableCollection<Question> Questions { get; } = new ObservableCollection<Question>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

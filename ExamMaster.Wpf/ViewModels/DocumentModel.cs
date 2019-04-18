using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ExamPaperParser.Number.Extractors.Exceptions;
using FormattedFileParser.Exceptions;

namespace ExamMaster.Wpf.ViewModels
{
    public class DocumentModel : INotifyPropertyChanged
    {
        private string _relativePath;
        public string RelativePath
        {
            get => _relativePath;
            set
            {
                _relativePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RelativePath)));
            }
        }

        private ObservableCollection<ParagraphFormatException> _exceptions;
        public ObservableCollection<ParagraphFormatException> Exceptions
        {
            get => _exceptions;
            set
            {
                _exceptions = value;

                ExceptionMessages = _exceptions.Select(o => o switch
                {
                    NumberException numberE => $"位于：{numberE.Position} 后\n{numberE.Message}\n{numberE.Content}",
                    _ => $"{o.Message}\n{o.Content}",
                }).ToList();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exceptions)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExceptionMessages)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExceptionLevel)));
            }
        }

        public List<string> ExceptionMessages { get; private set; } = new List<string>();

        public enum ExceptionLevelCasing
        {
            SevereError, Error, Warning, Info, None
        }

        public ExceptionLevelCasing ExceptionLevel
        {
            get
            {
                if (_exceptions.Count == 0)
                {
                    return ExceptionLevelCasing.None;
                }
                else if (_exceptions.Last() is SevereException)
                {
                    return ExceptionLevelCasing.SevereError;
                }
                else if (_exceptions.Count < 3)
                {
                    return ExceptionLevelCasing.Info;
                }
                else if (_exceptions.Count < 10)
                {
                    return ExceptionLevelCasing.Warning;
                }

                return ExceptionLevelCasing.Error;
            }
        }

        private ObservableCollection<DocumentSection> _sections;
        public ObservableCollection<DocumentSection> Sections
        {
            get => _sections;
            set
            {
                _sections = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sections)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DocumentModel(string relativePath)
        {
            _relativePath = relativePath;
            _exceptions = new ObservableCollection<ParagraphFormatException>();
            _sections = new ObservableCollection<DocumentSection>();
        }

        public DocumentModel(string relativePath, IEnumerable<ParagraphFormatException> exceptions, IEnumerable<DocumentSection> documentSections)
        {
            _relativePath = relativePath;
            _exceptions = new ObservableCollection<ParagraphFormatException>(exceptions);
            _sections = new ObservableCollection<DocumentSection>(documentSections);
        }
    }
}

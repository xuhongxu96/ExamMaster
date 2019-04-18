using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FormattedFileParser.Exceptions;

namespace ExamMaster.Wpf.Controls
{
    /// <summary>
    /// ExceptionList.xaml 的交互逻辑
    /// </summary>
    public partial class ExceptionList : UserControl
    {
        public ObservableCollection<ParagraphFormatException> Exceptions
        {
            get => (ObservableCollection<ParagraphFormatException>)GetValue(ExceptionsProperty);
            set => SetValue(ExceptionsProperty, value);
        }

        public static readonly DependencyProperty ExceptionsProperty
            = DependencyProperty.Register(
              nameof(Exceptions),
              typeof(ObservableCollection<ParagraphFormatException>),
              typeof(ExceptionList),
              new PropertyMetadata(new ObservableCollection<ParagraphFormatException>()));

        public ExceptionList()
        {
            InitializeComponent();
        }
    }
}

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
using ExamMaster.Wpf.ViewModels;

namespace ExamMaster.Wpf.Controls
{
    /// <summary>
    /// PaperList.xaml 的交互逻辑
    /// </summary>
    public partial class PaperList : UserControl
    {
        public DocumentModel SelectedDocument
        {
            get => (DocumentModel)GetValue(SelectedDocumentProperty);
            set => SetValue(SelectedDocumentProperty, value);
        }

        public static readonly DependencyProperty SelectedDocumentProperty
            = DependencyProperty.Register(
              nameof(SelectedDocument),
              typeof(DocumentModel),
              typeof(PaperList),
              new PropertyMetadata(new DocumentModel("")));

        public ObservableCollection<DocumentModel> Documents
        {
            get => (ObservableCollection<DocumentModel>)GetValue(DocumentsProperty);
            set => SetValue(DocumentsProperty, value);
        }

        public static readonly DependencyProperty DocumentsProperty
            = DependencyProperty.Register(
              nameof(Documents),
              typeof(ObservableCollection<DocumentModel>),
              typeof(PaperList),
              new PropertyMetadata(new ObservableCollection<DocumentModel>()));

        public PaperList()
        {
            InitializeComponent();
        }
    }
}

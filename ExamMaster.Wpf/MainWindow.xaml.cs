using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ExamMaster.Wpf.ViewModels;
using ExamPaperParser.Number.Extractors;
using ExamPaperParser.Number.Extractors.Exceptions;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Parsers.DecoratedNumberParsers;
using ExamPaperParser.Number.Parsers.NumberParsers;
using FormattedFileParser.Exceptions;
using FormattedFileParser.Models;
using FormattedFileParser.NumberingUtils.Converters;
using FormattedFileParser.Parsers.Docx;
using FormattedFileParser.Processors;

namespace ExamMaster.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AppModel AppModel { get; } = new AppModel();

        private static readonly UniversalNumberParser _numberParser = new UniversalNumberParser();
        private static readonly UniversalDecoratedNumberParser _decoratedNumberParser = new UniversalDecoratedNumberParser(_numberParser);
        private static readonly NumberExtractor _extractor = new NumberExtractor(_decoratedNumberParser);

        private static readonly List<IProcessor> _processors = new List<IProcessor>
        {
            new PrependNumberingToContentProcessor(new DefaultNumberingConverterRegistry()),
        };

        public MainWindow()
        {
            InitializeComponent();

            DataContext = AppModel;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog()
            {
                SelectedPath = AppModel.DocumentList.DirectoryPath,
            })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    AppModel.DocumentList.DirectoryPath = dialog.SelectedPath;
                    AppModel.DocumentList.Documents.Clear();

                    try
                    {
                        foreach (var item in Directory.EnumerateFileSystemEntries(
                            AppModel.DocumentList.DirectoryPath,
                            "*.docx", SearchOption.AllDirectories))
                        {
                            if (Path.GetFileName(item).StartsWith("~"))
                            {
                                continue;
                            }

                            var relativePath = Path.GetRelativePath(AppModel.DocumentList.DirectoryPath, item);
                            AppModel.DocumentList.Documents.Add(new DocumentModel(relativePath));
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("无权限访问此文件夹！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private Question ConvertNumberNodeToQuestion(NumberNode node)
        {
            var question = new Question
            {
                Header = node.DecoratedNumber.RawRepresentation + " " + node.Header,
                Body = node.Body,
            };

            foreach (var child in node.Children)
            {
                question.Questions.Add(ConvertNumberNodeToQuestion(child));
            }

            return question;
        }

        private IEnumerable<Question> ConvertNumberRootToQuestions(NumberRoot root)
        {
            foreach (var child in root.Children)
            {
                yield return ConvertNumberNodeToQuestion(child);
            }
        }

        private async Task<Tuple<List<ParagraphFormatException>, List<DocumentSection>>> LoadDocument(DocumentModel documentModel)
        {
            return await Task.Run(() =>
            {
                var filePath = Path.Combine(AppModel.DocumentList.DirectoryPath, documentModel.RelativePath);
                var sections = new List<DocumentSection>();
                var exceptions = new List<ParagraphFormatException>();

                try
                {
                    using (var parser = new DocxParser(filePath, _processors))
                    {
                        var doc = parser.Parse(out var parseExceptions);
                        exceptions.AddRange(parseExceptions);

                        foreach (var result in _extractor.Extract(doc))
                        {
                            (var sectionName, var questionRoot, var extractExceptions) = result;
                            exceptions.AddRange(extractExceptions);

                            sections.Add(new DocumentSection(sectionName, ConvertNumberRootToQuestions(questionRoot)));
                        }
                    }
                }
                catch (IOException e)
                {
                    var msg = e.Message;
                    if (e.Message.Contains("being used by another process"))
                    {
                        msg = "文件被占用（打开），请先关闭";
                    }

                    exceptions.Add(new SevereException(msg));
                }

                return Tuple.Create(exceptions, sections);
            }).ConfigureAwait(false);
        }

        private async void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDocumentText.Visibility = Visibility.Collapsed;
            StatusText.Visibility = Visibility.Visible;
            ProgressBar.Visibility = Visibility.Visible;

            ProgressBar.Maximum = AppModel.DocumentList.Documents.Count;
            int i = 0;

            foreach (var documentModel in AppModel.DocumentList.Documents)
            {
                (var exceptions, var sections) = await LoadDocument(documentModel);
                documentModel.Exceptions = new ObservableCollection<ParagraphFormatException>(exceptions);
                documentModel.Sections = new ObservableCollection<DocumentSection>(sections);

                StatusText.Text = documentModel.RelativePath;
                ProgressBar.Value = ++i;
            }

            ProgressBar.Visibility = Visibility.Collapsed;
            StatusText.Visibility = Visibility.Collapsed;
            SelectedDocumentText.Visibility = Visibility.Visible;

            MessageBox.Show("解析完成！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var path = Path.Combine(AppModel.DocumentList.DirectoryPath, AppModel.DocumentList.SelectedDocument.RelativePath);
            if (File.Exists(path))
            {
                new Process
                {
                    StartInfo = new ProcessStartInfo(path)
                    {
                        UseShellExecute = true,
                    }
                }.Start();
            }
            else
            {
                MessageBox.Show("文件不存在！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ReParseButton_Click(object sender, RoutedEventArgs e)
        {
            var documentModel = AppModel.DocumentList.SelectedDocument;

            (var exceptions, var sections) = await LoadDocument(documentModel);
            documentModel.Exceptions = new ObservableCollection<ParagraphFormatException>(exceptions);
            documentModel.Sections = new ObservableCollection<DocumentSection>(sections);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Parsers.DecoratedNumberParsers;
using ExamPaperParser.Number.Parsers.NumberParsers;
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
                            var relativePath = Path.GetRelativePath(AppModel.DocumentList.DirectoryPath, item);
                            AppModel.DocumentList.Documents.Add(new DocumentModel
                            {
                                RelativePath = relativePath,
                            });
                        }

                        if (AppModel.DocumentList.Documents.Any())
                        {
                            AppModel.CurrentDocument = AppModel.DocumentList.Documents[0];
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

        private async Task LoadDocument(DocumentModel documentModel)
        {
            var filePath = Path.Combine(AppModel.DocumentList.DirectoryPath, documentModel.RelativePath);
            documentModel.Exceptions.Clear();
            documentModel.Sections.Clear();

            GC.Collect();

            await Task.Run(() =>
            {
                using (var parser = new DocxParser(filePath, _processors))
                {
                    ParsedFile doc;
                    {
                        doc = parser.Parse(out var exceptions);
                        foreach (var exception in exceptions)
                        {
                            documentModel.Exceptions.Add(exception);
                        }
                    }

                    foreach (var item in _extractor.Extract(doc))
                    {
                        (var sectionName, var questionRoot, var exceptions) = item;

                        foreach (var exception in exceptions)
                        {
                            documentModel.Exceptions.Add(exception);
                        }

                        var section = new DocumentSection
                        {
                            Name = sectionName,
                        };

                        foreach (var question in ConvertNumberRootToQuestions(questionRoot))
                        {
                            section.Questions.Add(question);
                        }

                        documentModel.Sections.Add(section);
                    }
                }
            });
        }

        private async void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var documentModel in AppModel.DocumentList.Documents)
            {
                await LoadDocument(documentModel);
            }

            AppModel.DocumentList.Notify(nameof(DocumentList.Documents));
        }
    }
}

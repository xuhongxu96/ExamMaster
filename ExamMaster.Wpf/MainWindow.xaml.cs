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

namespace ExamMaster.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FileList FileList { get; } = new FileList();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog()
            {
                SelectedPath = FileList.DirectoryPath,
            })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileList.DirectoryPath = dialog.SelectedPath;
                    FileList.DocxFiles.Clear();
                    try
                    {
                        foreach (var item in Directory.EnumerateFileSystemEntries(
                            FileList.DirectoryPath,
                            "*.docx", SearchOption.AllDirectories))
                        {
                            var relativePath = Path.GetRelativePath(FileList.DirectoryPath, item);
                            FileList.DocxFiles.Add(relativePath);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("无权限访问此文件夹！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}

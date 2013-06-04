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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace ReportCreate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string OpenPathDefault = "";
        public const string SavePathDefault = "";

        ReportCreator reportCreator = new ReportCreator(OpenPathDefault, SavePathDefault);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            string[] files = Directory.GetFiles(fbd.SelectedPath);

            reportCreator.OpenPath = fbd.SelectedPath;
            reportCreator.FilePaths = files;

        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            reportCreator.SavePath = fbd.SelectedPath;

        }

        private void CalculateButtonClick(object sender, RoutedEventArgs e)
        {
            reportCreator.ReadFiles();
            reportCreator.GetScanReport();
            reportCreator.GetTotalReport();
        }
    }
}

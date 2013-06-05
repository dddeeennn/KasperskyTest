using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace ReportCreate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //Default Directories
        public static string OpenPathDefault
        {
            get { return Path.GetPathRoot(Environment.SystemDirectory) + "Test\\Sourse\\"; }
        }
        public static string SavePathDefault
        {
            get { return Path.GetPathRoot(Environment.SystemDirectory) + "Test\\Result\\"; }
        }
        //state programm for display
        private string state;
        public string State
        {
            get { return state; }
            set { state = value; OnPropertyChanged("State"); }
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitPathDefault();
        }

        //set directory with reports
        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            ReportCreator.OpenPath = fbd.SelectedPath;
        }
        //set directory for save report
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            ReportCreator.SavePath = fbd.SelectedPath;
        }
        //get reports
        private void CalculateButtonClick(object sender, RoutedEventArgs e)
        {
            ReportCreator reportCreator = new ReportCreator();
            try
            {
                reportCreator.ReadFiles();
                reportCreator.GetScanReport();
                reportCreator.GetTotalReport();
                State = "Report was crate! in " + ReportCreator.SavePath;
            }
            catch (Exception exc)
            {
                State = "Error!" + exc.Message;
            }
        }

        //Set default directory in report creator
        private void InitPathDefault()
        {
            if (!Directory.Exists(OpenPathDefault)) Directory.CreateDirectory(OpenPathDefault);
            if (!Directory.Exists(SavePathDefault)) Directory.CreateDirectory(SavePathDefault);
            ReportCreator.OpenPath = OpenPathDefault;
            ReportCreator.SavePath = SavePathDefault;
        }

        //event + implementation interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

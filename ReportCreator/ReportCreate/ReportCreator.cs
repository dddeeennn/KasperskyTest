using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportCreate
{
    class ReportCreator
    {
        public static string OpenPath { get; set; }
        public static string SavePath { get; set; }
        //list log files from open directory
        public List<LogFile> LogFiles { get; set; }
        //dictionary for storage total report
        private Dictionary<Guid, TotalReportItem> TotalReport { get; set; }
        public string[] FilePaths { get; set; }

        public ReportCreator()
        {
            FilePaths = Directory.GetFiles(OpenPath);
            LogFiles = new List<LogFile>();
            TotalReport = new Dictionary<Guid, TotalReportItem>();
        }

        /// <summary>
        /// read file from directory
        /// </summary>
        public void ReadFiles()
        {
            foreach (string filePath in FilePaths)
            {
                LogFiles.Add(new LogFile(filePath));
            }
        }
        
        public void GetScanReport()
        {
            string pathString = SavePath + "\\ScanReport.csv";
            if (!File.Exists(pathString))
            {
                FileStream fs = File.Create(pathString);
                fs.Close();
            }
            //get collection of logItems
            List<LogItem> listForSerilize = new List<LogItem>();
            foreach (LogFile item in LogFiles)
            {
                listForSerilize.AddRange(item.LogItems);
            }
            //write in file collection
            Serialize(listForSerilize, pathString);
        }
       
        public void GetTotalReport()
        {
            //for each files merge total reports
            foreach (LogFile logFile in LogFiles)
            {
                foreach (KeyValuePair<Guid, TotalReportItem> item in logFile.GetTotalReportFile())
                {
                    if (TotalReport.ContainsKey(item.Key))
                    {
                        TotalReport[item.Key].UserCount++;
                        TotalReport[item.Key].FileCount += item.Value.FileCount;
                    }
                    else
                    {
                        TotalReport.Add(item.Key, new TotalReportItem(item.Key, item.Value.UserCount, item.Value.FileCount));
                    }
                }
            }
            string pathString = SavePath + "\\TotalReport.csv";
            //get list for serialize
            List<TotalReportItem> list = new List<TotalReportItem>();
            list = TotalReport.Values.ToList();
            //sort descending
            list.Sort((x,y)=>x.UserCount<y.UserCount?1:x.UserCount==y.UserCount?0:-1);
            //write in file
            Serialize(list,pathString);
        }

        /// <summary>
        /// generic method for serialize List
        /// </summary>
        /// <typeparam name="T">generic type must override ToString</typeparam>
        /// <param name="data">Data Array</param>
        /// <param name="path">path to write</param>
        private void Serialize<T>(List<T> data, string path)
        {
            if (File.Exists(path)) File.Delete(path);
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                foreach (T item in data)
                {
                    streamWriter.WriteLine(item.ToString());
                }
            }
        }
        
    }
}

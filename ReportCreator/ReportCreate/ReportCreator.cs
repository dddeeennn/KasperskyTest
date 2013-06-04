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
        public string OpenPath { get; set; }
        public string SavePath { get; set; }
        public List<LogFile> LogFiles { get; set; }
        private Dictionary<Guid, TotalReportItem> TotalReport { get; set; }
        public string[] FilePaths { get; set; }

        public ReportCreator(string openPath, string savePath)
        {
            OpenPath = openPath;
            SavePath = savePath;
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
        /// <summary>
        /// write in file scan report
        /// </summary>
        public void GetScanReport()
        {
            string pathString = SavePath + "\\ScanReport.csv";
            if (!File.Exists(pathString))
            {
                FileStream fs = File.Create(pathString);
                fs.Close();
            }
            List<LogItem> listForSerilize = new List<LogItem>();
            foreach (LogFile item in LogFiles)
            {
                listForSerilize.AddRange(item.LogItems);
            }
            Serialize(listForSerilize, pathString);
        }

        /// <summary>
        /// generic method for serialize List
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="data">Data Array</param>
        /// <param name="path">path to write</param>
        private void Serialize<T>(List<T> data, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                foreach (T item in data)
                {
                    string line = "";
                    //get property of item use reflection 
                    Type type = typeof(T);
                    PropertyInfo[] propInfo = type.GetProperties();
                    //for each property value write line in file
                    for (int i = 0; i < propInfo.Length; i++)
                    {
                        //change bool value to "1" or "0"
                        if (propInfo[i].PropertyType == typeof(bool))
                        {
                            line += (bool)propInfo[i].GetValue(item) ? 1 : 0;
                        }
                        else if (propInfo[i].PropertyType == typeof(Guid))
                        {
                            //write Guid in special format
                            Guid tmpGuid = (Guid)propInfo[i].GetValue(item);
                            line += tmpGuid.ToString("N");
                        }
                        else
                        {
                            line += propInfo[i].GetValue(item);
                        }
                        if (i != propInfo.Length - 1) line += ";";
                    }
                    streamWriter.WriteLine(line);
                }
            }
        }

        public void GetTotalReport()
        {
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
            List<TotalReportItem> list = new List<TotalReportItem>();
            list = TotalReport.Values.ToList();
            list.Sort((x,y)=>x.UserCount<y.UserCount?1:x.UserCount==y.UserCount?0:-1);
            Serialize(list,pathString);
        }
    }
}

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
        public string[] FilePaths { get; set; }

        public ReportCreator(string openPath, string savePath)
        {
            OpenPath = openPath;
            SavePath = savePath;
            LogFiles = new List<LogFile>();
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
                    for (int i = 0; i < propInfo.Length;i++ )
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

        }
    }
}

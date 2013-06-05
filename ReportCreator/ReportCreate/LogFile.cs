using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportCreate
{
    class LogFile
    {
        public string FilePath { get; set; }
        //list item of log file
        public List<LogItem> LogItems { get; private set; }
        //dictionary for storage total report item with unique hash
        public Dictionary<Guid, TotalReportItem> TotalReportFile { get; private set; }

        public LogFile(string path)
        {
            FilePath = path;
            LogItems = new List<LogItem>();
            TotalReportFile = new Dictionary<Guid, TotalReportItem>();
            ReadLogData();
        }
        //create totalReport for this log file and calculate fileCount 
        public Dictionary<Guid, TotalReportItem> GetTotalReportFile()
        {
            foreach (LogItem logItem in LogItems)
            {
                if (TotalReportFile.ContainsKey(logItem.Hash)) TotalReportFile[logItem.Hash].FileCount++;
                else TotalReportFile.Add(logItem.Hash, new TotalReportItem(logItem.Hash, 1, 1));
            }
            return TotalReportFile;
        }
        //read data from file and write in list
        private void ReadLogData()
        {
            using (StreamReader streamReader = new StreamReader(FilePath))
            {
                while (!streamReader.EndOfStream)
                {
                    string userId = Path.GetFileName(FilePath).Substring(0, 5);
                    string line = streamReader.ReadLine();
                    String[] arrData = line.Split(';');
                    string fileName = arrData[1];
                    bool isVirus = arrData[3] == "1";
                    Guid hash = new Guid(arrData[0]);
                    long fileSize;
                    Int64.TryParse(arrData[2], out  fileSize);
                    LogItems.Add(new LogItem() { UserId = userId, FileName = fileName, IsVirus = isVirus, Hash = hash, FileSize = fileSize });
                }
            }
        }
    }
}

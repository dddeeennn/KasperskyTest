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
        public List<LogItem> LogItems { get; private set; }
        public List<TotalReportItem> TotalReportList { get; set; }

        public LogFile(string path)
        {
            FilePath = path;
            LogItems = new List<LogItem>();
            TotalReportList = new List<TotalReportItem>();
            ReadLogData();
        }

        private void ReadLogData()
        {
            try
            {
                    using (StreamReader streamReader = new StreamReader(FilePath))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            string userId = Path.GetFileName(FilePath).Substring(0,5);
                            string line = streamReader.ReadLine();
                            String[] arrData = line.Split(';');
                            string fileName = arrData[1];
                            bool isVirus = arrData[3] == "1";
                            Guid hash = new Guid(arrData[0]);
                            long fileSize;
                            Int64.TryParse(arrData[2], out  fileSize);
                            LogItems.Add(new LogItem(userId, fileName, isVirus, hash, fileSize));
                        }
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception message : " + e.Message);
            }
        }
    }
}

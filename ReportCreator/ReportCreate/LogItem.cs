using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportCreate
{
    struct LogItem
    {
        public string UserId { get; set; }
        public string FileName { get; set; }
        public bool IsVirus { get; set; }
        public Guid Hash { get; set; }
        public Int64 FileSize { get; set; }

        //override for serialize
        public override string ToString()
        {
            string lineString ="";
            lineString += UserId + ";" + FileName + ";";
            lineString += IsVirus ? "1" : "0";
            lineString += ";" + Hash.ToString("N") + ";" + FileSize;
            return lineString;
        }
    }
}

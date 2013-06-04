using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportCreate
{
    class LogItem
    {
        public string UserId { get; set; }
        public string FileName { get; set; }
        public bool IsVirus { get; set; }
        public Guid Hash { get; set; }
        public Int64 FileSize { get; set; }

        public LogItem(string userId,string fileName,bool isVirus,Guid hash,Int64 fileSize)
        {
            UserId = userId;
            FileName = fileName;
            IsVirus = isVirus;
            Hash = hash;
            FileSize = fileSize;
        }
    }
}

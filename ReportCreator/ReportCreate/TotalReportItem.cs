using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportCreate
{
    class TotalReportItem
    {
        public Guid Hash { get; set; }
        public int UserCount { get; set; }
        public int FileCount { get; set; }

        public TotalReportItem (Guid hash , int userCount,int fileCount)
        {
            Hash = hash;
            UserCount = userCount;
            FileCount = fileCount;
        }
    }
}

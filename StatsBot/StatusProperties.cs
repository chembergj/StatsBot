using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsBot
{
    [Serializable]
    public class StatusProperties
    {
        public int LastLineNumberProcess { get; set; }      // 0=not started or last run completed
        public string LastActiveInputFilename { get; set; }
    }
}

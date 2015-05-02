using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsBot
{
    class MemberStatusWriter
    {
        readonly string  filename;
        
        public MemberStatusWriter(string filename)
        {
            this.filename = filename;
        }

        public void WriteStatus(string cid, string status)
        {
            using (TextWriter tw = new StreamWriter(filename, true))
            {
                tw.WriteLine(cid + "," + status);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsBot
{
    public class MemberStatus
    {
        public string CID { get; set; }
        public bool Active { get; set; }
    }

    class MembersListProvider
    {
        public string Filename { get; set; }
        
        public List<MemberStatus> GetMembersList()
        {
            var result = new List<MemberStatus>();

            using (StreamReader oStreamReader = new StreamReader(File.OpenRead(Filename)))
            {
                while (!oStreamReader.EndOfStream)
                {
                    var splittedLine = oStreamReader.ReadLine().Split(',');
                    result.Add(new MemberStatus() { CID = splittedLine[0], Active = splittedLine[1].ToLower() == "yes" });
                }
            }

            return result;
/*
            return new List<MemberStatus>() {
                new MemberStatus() { CID = "1138158", Rating = "C1", Active = true},
                new MemberStatus() { CID = "880543", Rating = "C3", Active = true},
                new MemberStatus() { CID = "862571", Rating = "C3", Active = true}
            };                */
        }
    }
}

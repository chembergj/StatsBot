using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsBot
{
    public class MemberStatus
    {
        public string CID { get; set; }
        public string Rating { get; set; }
        public bool Active { get; set; }
    }

    class MembersListProvider
    {
        public List<MemberStatus> GetMembersList()
        {
            return new List<MemberStatus>() {
                new MemberStatus() { CID = "1138158", Rating = "C1", Active = true},
                new MemberStatus() { CID = "880543", Rating = "C3", Active = true},
                new MemberStatus() { CID = "862571", Rating = "C3", Active = true}
            };                
        }
    }
}

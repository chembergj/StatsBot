using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsBot
{
    class StatsParser
    {
        public static TimeSpan GetTotalHours(string htmlText)
        {
            TimeSpan atcHours = TimeSpan.Zero;
            TimeSpan admHours = TimeSpan.Zero;

            int startIdx = htmlText.IndexOf("<DIV class=box>\r\n<H2>ATC Ratings for </H2>");
            if (startIdx > 0)
            {

                int endIdx = htmlText.IndexOf("</TR></TBODY></TABLE></DIV>", startIdx);
                string activityBoxText = htmlText.Substring(startIdx, endIdx + 27 - startIdx);

                var s2Hours = GetHours(activityBoxText, "Student 2");
                var s3Hours = GetHours(activityBoxText, "Student 3");
                var c1Hours = GetHours(activityBoxText, "Controller");
                var c3Hours = GetHours(activityBoxText, "Senior Controller");
                var i1Hours = GetHours(activityBoxText, "Instructor");
                var i3Hours = GetHours(activityBoxText, "Senior Instructor");
                atcHours = s2Hours.Add(s3Hours).Add(c1Hours).Add(c3Hours).Add(i1Hours).Add(i3Hours);
            }

            startIdx = htmlText.IndexOf("<DIV class=box>\r\n<H2>Administrative Ratings for </H2>");
            if (startIdx > 0)
            {
                int endIdx = htmlText.IndexOf("</TR></TBODY></TABLE></DIV>", startIdx);
                string activityBoxText = htmlText.Substring(startIdx, endIdx + 27 - startIdx);
                admHours = GetAdminHours(activityBoxText, "Administrator");
            }

            return atcHours.Add(admHours);  
        }

        private static TimeSpan GetHours(string activityBoxText, string rating)
        {
            int ratingIdx = activityBoxText.IndexOf("<TD>" + rating);
            if (ratingIdx == -1) { return TimeSpan.Zero;  }
            int timeIdx = activityBoxText.IndexOf("<TD>", ratingIdx + 4) + 4;
            int endTimeIdx = activityBoxText.IndexOf("</TD>", timeIdx);

            var timeSplit = activityBoxText.Substring(timeIdx, endTimeIdx - timeIdx).Split(':');

            var res = TimeSpan.FromHours(int.Parse(timeSplit[0]));
            return res.Add(TimeSpan.FromMinutes(int.Parse(timeSplit[1])));
        }

        private static TimeSpan GetAdminHours(string activityBoxText, string rating)
        {
            int ratingIdx = activityBoxText.IndexOf("<TD>" + rating);
            if (ratingIdx == -1) { return TimeSpan.Zero; }
            int timeIdx = activityBoxText.IndexOf("<TD>", ratingIdx + 4) + 4;
            int endTimeIdx = activityBoxText.IndexOf("</TD>", timeIdx);

            var timeSplit = activityBoxText.Substring(timeIdx, endTimeIdx - timeIdx).Split(':');

            var res = TimeSpan.FromHours(int.Parse(timeSplit[0]));
            return res.Add(TimeSpan.FromMinutes(int.Parse(timeSplit[1])));
        }

    }
}

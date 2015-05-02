using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace StatsBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum State { Init, JustLoggedIn, Processing };
        
        State state = State.Init;
        MemberStatusWriter writer;
        MemberStatusWriter diffwriter;
        StatusProperties properties;
        List<MemberStatus>.Enumerator memberEnumerator;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            browser.Navigate("http://stats.vatsim.net/conn_details_time.php?id=" + memberEnumerator.Current.CID  + "&timeframe=6_months");
            state = State.JustLoggedIn;
            properties = new StatusProperties() { LastLineNumberProcess = 0 };
        }

        private void browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (state == State.JustLoggedIn)
            {
                textBlock.Text += "Processing member list...\n";
                writer = new MemberStatusWriter("memberstatus.csv");
                diffwriter = new MemberStatusWriter("memberstatus_changed.csv");
                state = State.Processing;
            }
            
            if(state == State.Processing) 
            { 
                dynamic doc = browser.Document;
                string htmlText = (string)doc.documentElement.InnerHtml;
                
                var total = StatsParser.GetTotalHours(htmlText);
                bool active = (total.Hours >= 10); 
                this.writer.WriteStatus(memberEnumerator.Current.CID, active ? "active" : "inactive");
                if (!active && memberEnumerator.Current.Active)
                {
                    // Status changed from active->inactive, write in diff file too
                    diffwriter.WriteStatus(memberEnumerator.Current.CID, active ? "active" : "inactive");
                }

                // Update status file
                properties.LastLineNumberProcess++;
                var propswriter = new System.Xml.Serialization.XmlSerializer(typeof(StatusProperties));

                System.IO.StreamWriter file = new System.IO.StreamWriter(
                    @"statusproperties.xml");
                propswriter.Serialize(file, properties);
                file.Close();

                if (memberEnumerator.MoveNext())
                {
                    browser.Navigate("http://stats.vatsim.net/conn_details_time.php?id=" + memberEnumerator.Current.CID + "&timeframe=6_months");
                }
                else
                {
                    textBlock.Text += "Processing completed!\n";
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBlock.Text += "Reading member list\n";
            var listProvider = new MembersListProvider();
            memberEnumerator = listProvider.GetMembersList().GetEnumerator();
            memberEnumerator.MoveNext();
            textBlock.Text += "Reading member list done\nNow log on to stats.vatsim.net and click the button below when done\n";
        }
    }
}

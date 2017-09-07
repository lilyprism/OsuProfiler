using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Windows.Threading;

namespace OsuProfiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer timer = new DispatcherTimer();
        public osu_api.osuAPI api;
        public osu_api.User user;
        public osu_api.User oldUsr;
        int dRank = 0;
        int dPlays = 0;
        double dPP = 0.00;
        int dLvl = 0;
        double dAcc = 0.00;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += Timer_Tick;
            api = new osu_api.osuAPI("4a98a511a32966282204aa2638e461ee65b97c0e");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            user = api.GetUser(username.Text, (osu_api.Mode)osuMode.SelectedIndex);
            UpdateValues(0);
        }

        private void uptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                stopBtn.IsEnabled = true;
                uptBtn.IsEnabled = false;
            }
            user = api.GetUser(username.Text, (osu_api.Mode)osuMode.SelectedIndex);
            UpdateValues(1);
        }

        public void UpdateValues(int i = 0)
        {
            if (oldUsr!= null)
            {
                dRank += oldUsr.pp_rank - user.pp_rank;
                dPlays += user.playcount - oldUsr.playcount;
                dPP += user.pp_raw - oldUsr.pp_raw;
                dLvl += (int)user.level - (int)oldUsr.level;
                dAcc += user.accuracy - oldUsr.accuracy;
            }
            userRank.Text = "Rank: " + user.pp_rank.ToString("N0") + " ( " + dRank.ToString("N0") + " )";
            userPlays.Text = "Plays: " + user.playcount.ToString("N0") + " ( " + dPlays.ToString("N0") + " )";
            userPR.Text = "PP: " + user.pp_raw.ToString("#.##") + " ( " + dPP.ToString("#.##") + " )";
            userLvl.Text = "Level: " + ((int)(user.level)).ToString("N0") + " ( " + dLvl.ToString("N0") + " )";
            userAcc.Text = "Accuracy: " + user.accuracy.ToString("#.##") + "%" + " ( " + dAcc.ToString("#.##") + " )";
            userCountry.Text = "Country: " + user.country;

            if (i == 1) UpdateImg();

            oldUsr = user;
        }

        private void UpdateImg()
        {
            string url = "https://a.ppy.sh/" + user.user_id;
            var converter = new ImageSourceConverter();
            userImg.Source =
                     (ImageSource)converter.ConvertFromString(url);
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                stopBtn.IsEnabled = false;
                uptBtn.IsEnabled = true;
            }
        }
    }
}

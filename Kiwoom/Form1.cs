using AxKHOpenAPILib;
using Kiwoom.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AdsJumboWinForm;

namespace Kiwoom
{
    public partial class Kiwoom : Form
    {
        private KiwoomManager kiwoom;
        private TelegramManager bot;
        private String api;
 



        public Kiwoom()
        {
            InitializeComponent();

            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            kiwoom = new KiwoomManager(this.axKHOpenAPI1);
            //bot = new TelegramManager("1345119034:AAEvGVBDn0vAHEm720Rb-sA-COgTzN6yb3A");
            bot = new TelegramManager("1342784362:AAGwyO_SMPawMNvV1b8YsuXSyQUjE4FZNgk");

        }


        private void onClick로그인버튼(object sender, EventArgs e)
        {
           kiwoom.CommConnect();
        }

        private void onClick조건식가져오기(object sender, EventArgs e)
        {
            kiwoom.GetUserJogun();
        }

        private void Kiwoom_Load(object sender, EventArgs e)
        {
            kiwoom.LinkWithTelegram(bot);
            bot.LinkWithKiwoom(kiwoom);

            bannerAds1.ShowAd(728, 90, "7gq1czmtoxew");
        }
    }
}

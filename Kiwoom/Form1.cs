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
    public partial class Kiwoom :Form
    {
        public static Kiwoom kiwoomform;
        private KiwoomManager kiwoom;
        private TelegramManager bot;

        public Kiwoom()
        {
            try {
                InitializeComponent();
                kiwoomform = this;
                System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

                kiwoom = new KiwoomManager(this.axKHOpenAPI1);

                //현규
                bot = new TelegramManager("1344582178:AAGWKBtgC43v8pS-gsUo5Q28Ii6zyAsrE-0");

                //한주
                //bot = new TelegramManager("1342784362:AAGwyO_SMPawMNvV1b8YsuXSyQUjE4FZNgk");

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            
        }

        public void AddLog(string message)
        {
            try { this.log.Items.Add(message); }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            
        }

        private void onClick로그인버튼(object sender, EventArgs e)
        {
            try { 
                kiwoom.CommConnect();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            
        }

        private void Kiwoom_Load(object sender, EventArgs e)
        {
            try {
                kiwoom.LinkWithTelegram(bot);
                bot.LinkWithKiwoom(kiwoom);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            
           // bannerAds1.ShowAd(728, 90, "7gq1czmtoxew");
        }
    }
}

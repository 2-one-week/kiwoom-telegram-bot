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



namespace Kiwoom
{
    public partial class Kiwoom : Form
    {
        private KiwoomManager kiwoom;
        private TelegramManager bot;
        private String api;
        private long chatId;
        public Kiwoom()
        {
            InitializeComponent();

            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            kiwoom = new KiwoomManager(this.axKHOpenAPI1);
            bot = new TelegramManager("1342784362:AAGwyO_SMPawMNvV1b8YsuXSyQUjE4FZNgk");

            kiwoom.LinkWithTelegram(bot);
            bot.LinkWithKiwoom(kiwoom);
        }


        private void onClick로그인버튼(object sender, EventArgs e)
        {
            kiwoom.CommConnect();
        }

        private void onClick조건식가져오기(object sender, EventArgs e)
        {
            kiwoom.GetUserJogun();
        }


    }
}

using SocketIOClient.EventArguments;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//정규 표현식 이용
using System.Text.RegularExpressions;


///텔레그램 dll using 
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace Kiwoom.Network
{
    public class TelegramManager
    {
        private enum JogunState
        {
            start,
            stop,
            none
        }

        private TelegramBotClient bot;
        private KiwoomManager kiwoom;
        private ChatId chatId;

        private static Dictionary<ChatId, JogunState> jogunState = new Dictionary<ChatId, JogunState>();

        public TelegramManager(string accessToken)
        {
            bot = new Telegram.Bot.TelegramBotClient(accessToken);
            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
        }

        public void LinkWithKiwoom( KiwoomManager kiwoomManager)
        {
            kiwoom = kiwoomManager;
        }

        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text) return;

            if (message.Text.StartsWith("/로그인"))
            {
                kiwoom.CommConnect();
            }

            else if (message.Text.StartsWith("/알림시작"))
            {
                chatId = message.Chat.Id;
                await SendMessage("실시간 알림을 시작합니다.");
            }

            else if (message.Text.StartsWith("/알림종료"))
            {
                await SendMessage("실시간 알림을 종료합니다.");
                chatId = null;
            }

            else if (message.Text.StartsWith("/조건식리스트"))
            {
                kiwoom.GetUserJogun();
            }

            else if (message.Text.StartsWith("/감시리스트"))
            {
                kiwoom.GetWatchJogun();
            }

            else if (Regex.IsMatch(message.Text, "/실행 [0-9]+"))
            {
                kiwoom.StartJogun(int.Parse(Regex.Replace(message.Text, "/실행 ", "")));
            }

            else if (Regex.IsMatch(message.Text, "/중단 [0-9]+"))
            {
                kiwoom.StopJogun(int.Parse(Regex.Replace(message.Text, "/중단 ", "")));
            }

            else if (message.Text.StartsWith("/help")) 
            {
                String 도움말 = "[생활비 벌자 도움말]\n";
                await SendMessage(도움말);
            }

            else if (message.Text.StartsWith("/연결상태"))
            {
                int status = kiwoom.ConnectState();
                if(status == 0)
                {
                    await SendMessage("키움과 연결되어 있지 않습니다.");
                }else if(status == 1)
                {
                    await SendMessage("키움과 연결되어 있습니다.");
                }
            }

            else
            {
                await SendMessage("잘못된 명령어 입니다.\n다시 입력해 주세요!\n앞에 '/'를 붙였나 확인해주세요~");
            }

            
        }

        public async Task SendMessage(string data)
        {
            if(chatId != null)
            {
                await bot.SendTextMessageAsync(chatId, data);
            }
        }
    }
    
}

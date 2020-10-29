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

using System.IO;

namespace Kiwoom.Network
{
    public class TelegramManager
    {
        private TelegramBotClient bot;
        private KiwoomManager kiwoom;
        //한주
        private long Admin = 1304601666;

        //현규
        //private long Admin = 1035537988;
        private ChatId chatId = null;
        private List<ChatId> users = new List<ChatId>();

        public TelegramManager(string accessToken)
        {
            try
            {
                bot = new Telegram.Bot.TelegramBotClient(accessToken);
                bot.OnMessage += Bot_OnMessage;
                bot.StartReceiving();
                텔레그램User파일생성();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        private void 텔레그램User파일생성()
        {
            try
            {
                string strFile = System.Environment.CurrentDirectory + @"\userdata.ini";
                FileInfo fileInfo = new FileInfo(strFile);
                if (!fileInfo.Exists)
                {
                    FileStream TelegramApiFile = System.IO.File.Create(strFile);     // 파일생성
                    TelegramApiFile.Close();
                    return;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            
        }

        private void 텔레그램User제거(ChatId chatid)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + @"\userdata.ini");
                using (StreamWriter sr = new StreamWriter(System.Environment.CurrentDirectory + @"\userdata.ini"))
                {
                    foreach (string line in lines)
                    {
                        if(line != chatid)
                        {
                            sr.WriteLine(line);
                        }
                        
                    }
                    sr.Close();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }
        private async void 텔레그램User쓰기(ChatId chatid)
        {
            try
            {
                if(long.Parse(chatid) == Admin)
                {
                    return;
                }
                int isExist = 0;
                string[] lines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + @"\userdata.ini");
                using (StreamWriter sr = new StreamWriter(System.Environment.CurrentDirectory + @"\userdata.ini"))
                {
                    foreach (string line in lines)
                    {
                        if(line == chatid)
                        {
                            isExist = 1;
                        }
                        sr.WriteLine(line);
                    }
                    if(isExist == 0)
                    {
                        sr.WriteLine(chatid);
                        await SendMessageToPeople(long.Parse(chatid), "정상적으로 유저 등록되었습니다.");
                    }              
                    sr.Close();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        public void LinkWithKiwoom( KiwoomManager kiwoomManager)
        {
            try { kiwoom = kiwoomManager; }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try {
                var message = e.Message;
                if (message == null || message.Type != MessageType.Text) return;

                if (message.Text.StartsWith("/로그인"))
                {
                    if(message.Chat.Id != Admin)
                    {
                        await SendMessageToPeople(message.Chat.Id, "권한이 없습니다.");
                        return;
                    }

                    kiwoom.CommConnect();
                }

                else if (message.Text.StartsWith("/알림시작"))
                {
                    텔레그램User쓰기(message.Chat.Id);
                    await SendMessageToPeople(message.Chat.Id, "실시간 알림을 시작합니다.");
                    return;
                }

                else if (message.Text.StartsWith("/알림종료"))
                {
                    텔레그램User제거(message.Chat.Id);
                    await SendMessageToPeople(message.Chat.Id, "실시간 알림을 종료합니다.");
                    return;
                }

                else if ((message.Text.StartsWith("/조건식리스트") || message.Text.StartsWith("/조건식")))
                {
                    kiwoom.GetUserJogun(message.Chat.Id);
                    return;
                }

                else if (message.Text.StartsWith("/감시리스트") || message.Text.StartsWith("/감시"))
                {
                    kiwoom.GetWatchJogun(message.Chat.Id);
                    return;
                }

                else if ((Regex.IsMatch(message.Text, "/실행 [0-9]+")))
                {
                    if (message.Chat.Id != Admin)
                    {
                        await SendMessageToPeople(message.Chat.Id, "권한이 없습니다.");
                        return;
                    }
                    kiwoom.StartJogun(Admin, int.Parse(Regex.Replace(message.Text, "/실행 ", "")));
                    return;
                }

                else if ((Regex.IsMatch(message.Text, "/중단 [0-9]+")))
                {
                    if (message.Chat.Id != Admin)
                    {
                        await SendMessageToPeople(message.Chat.Id, "권한이 없습니다.");
                        return;
                    }
                    kiwoom.StopJogun(Admin,int.Parse(Regex.Replace(message.Text, "/중단 ", "")));
                    return;
                }

                else if (message.Text.StartsWith("/help") || message.Text.StartsWith("/start") || message.Text.StartsWith("/시작") || message.Text.StartsWith("/도움말") || message.Text.StartsWith("/명령어"))
                {
                    String 도움말 = "[생활비 벌자 명령어]\n\n";
                    도움말 += "[연결 및 로그인 관련]\n";
                    도움말 += "'/로그인' : 자신의 컴퓨터로 로그인하도록 명령합니다.\n";
                    도움말 += "'/연결상태' : 자신의 컴퓨터와 키움 OpenApi의 연결상태를 알려준다.\n";
                    도움말 += "'/알림시작' : Telegram 알림을 시작합니다\n'/알림종료' : Telegram 알림을 종료합니다.\n\n";

                    도움말 += "[조건식 관련]\n";
                    도움말 += "'/조건식 /조건식리스트' : 사용자가 HTS에 등록해둔 조건식을 불러온다.\n";
                    도움말 += "'/감시 /감시리스트' : 사용자가 실시간 감지를 실행한 조건식을 불러온다.\n";
                    도움말 += "'/실행 (조건식 번호)' : 해당 조건식 번호 조건식의 실시간 감지를 실행한다.\nex) /실행 2\n";
                    도움말 += "'/중단 (조건식 번호)' : 해당 조건식 번호 조건식의 실시간 감지를 중단한다.\nex) /중단 2\n\n";
                    도움말 += "$$주의$$\n";
                    도움말 += "'*알림을 받고 싶다면, 항상 '/알림시작'을 해주세요!*'\n\n";
                    도움말 += "$$꿀팁$$\n";
                    도움말 += "조건식을 불러오기 귀찮다면, /실행 (숫자)를 두번 실행하자.";
                    await SendMessageToPeople(message.Chat.Id,도움말);
                    return;
                }

                else if (message.Text.StartsWith("/연결상태"))
                {
                    int status = kiwoom.ConnectState();
                    if (status == 0)
                    {
                        await SendMessageToPeople(message.Chat.Id, "키움과 연결되어 있지 않습니다.");

                    }
                    else if (status == 1)
                    {
                        await SendMessageToPeople(message.Chat.Id,"키움과 연결되어 있습니다.");
                    }
                }

                else if(chatId != null)
                {
                    await SendMessageToPeople(message.Chat.Id, "잘못된 명령어 입니다.\n다시 입력해 주세요!\n앞에 '/'를 붙였나 확인해주세요~");
                }
                else if (message.Text.StartsWith("/관리자")){
                    string botMessageText = Regex.Replace(message.Text, "/관리자 ", "[봇 관리자가 전송]\n");
                    await SendMessageToAllExceptAdmin(botMessageText);
                }
                else
                {
                    await SendMessageToPeople(message.Chat.Id, "로그인이 필요합니다.");
                }

            }
            catch (Exception Ex)
            {
                await SendMessage("오류가 발생했습니다.\n오류가 발생했습니다.\n오류가 발생했습니다.\n팀뷰어로 껏다가 켜주세요.");
                Console.WriteLine(Ex);
            }
        }

        public async Task SendMessage(string data)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + @"\userdata.ini");
                await bot.SendTextMessageAsync(Admin, data);

                for (var i=0; i < lines.Length; i++)
                {
                    if(i==0&&chatId == null)
                    {
                        chatId = lines[i];
                    }
                    await bot.SendTextMessageAsync(lines[i], data);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        public async Task SendMessageToAllExceptAdmin(string data)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + @"\userdata.ini");

                for (var i = 0; i < lines.Length; i++)
                {
                    if (i == 0 && chatId == null)
                    {
                        chatId = lines[i];
                    }
                    await bot.SendTextMessageAsync(lines[i], data);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        public async Task SendMessageToAdmin(string data)
        {
            try
            {
                await bot.SendTextMessageAsync(Admin, data);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        
        public async Task SendMessageToPeople(long id, string data)
        {
            try
            {
                await bot.SendTextMessageAsync(id, data);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }
    }
}

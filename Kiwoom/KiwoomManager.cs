using AxKHOpenAPILib;
using Kiwoom.Models;
using Kiwoom.Network;
using Microsoft.SqlServer.Server;
using SocketIOClient.EventArguments;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kiwoom
{
    public class KiwoomManager
    {
        private AxKHOpenAPILib.AxKHOpenAPI api;
        private TelegramManager bot;

        private int _scrNum = 5000;

        private string 유저이름;

        private List<조건식> 조건식목록;
        private const int DELAY_TIME = 120;


        public KiwoomManager(AxKHOpenAPILib.AxKHOpenAPI api)
        {
            this.api = api;
            //this.bot = new TelegramManager(telegramToken, this)
            api.OnEventConnect += onEventConnect;
            api.OnReceiveTrData += onReceiveTrData;
            api.OnReceiveTrCondition += onReceiveTrCondition;
            api.OnReceiveRealData += onReceiveRealData;
            api.OnReceiveConditionVer += onReceiveConditionVer;
            api.OnReceiveRealCondition += onReceiveRealCondition;
        }

        private string GetScrNum()
        {
            if (_scrNum < 9999)
                _scrNum++;
            else
                _scrNum = 5000;
            return _scrNum.ToString();
        }


        public void LinkWithTelegram(TelegramManager telegramManager)
        {
            bot = telegramManager;
        }

        private async void onReceiveTrData(object sender, _DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveTrData", e.sRecordName));

            if (e.sRQName == "조건검색종목")
            {
                int count = api.GetRepeatCnt(e.sTrCode, e.sRQName);//요청의 반복 횟수를 요청합니다.
                for (int i = 0; i < count; i++)
                {
                        String 조건검색식종목메세지 = "[조건식 검색 종목 알림]\n";
                        string 종목코드 = api.GetCommData(e.sTrCode, e.sRQName, i, "종목코드").Trim(); ;
                        string 종목명 = api.GetCommData(e.sTrCode, e.sRQName, i, "종목명").Trim(); ;
                        string 포착가 = api.GetCommData(e.sTrCode, e.sRQName, i, "현재가").Trim(); ;
                        string 거래대금 = api.GetCommData(e.sTrCode, e.sRQName, i, "거래대금").Trim(); ;
                        string 거래량 = api.GetCommData(e.sTrCode, e.sRQName, i, "거래량").Trim(); ;
                        //string 포착가 = String.Format("{0:#,#}", api.GetCommData(e.sTrCode, e.sRQName, i, "현재가").Trim());
                       // string 거래대금 = String.Format("{0:#,#}", int.Parse(api.GetCommData(e.sTrCode, e.sRQName, i, "거래대금").Trim()));
                       // string 거래량 = String.Format("{0:#,#}", int.Parse(api.GetCommData(e.sTrCode, e.sRQName, i, "거래량").Trim()));

                        Console.WriteLine(종목코드, 종목명);

                        조건검색식종목메세지 += String.Format("{0} ({1}) 편입\n", 종목명, 종목코드);
                        조건검색식종목메세지 += String.Format("편입 시간 : {0}\n", TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Korea Standard Time").ToString());
                        조건검색식종목메세지 += String.Format("포착가 : {0}원\n", 포착가);
                        조건검색식종목메세지 += String.Format("거래량 : {0}\n", 거래량);
                        조건검색식종목메세지 += String.Format("거래대금 : {0}원\n", 거래대금);
                        조건검색식종목메세지 += String.Format("https://ssl.pstatic.net/imgfinance/chart/item/area/day/{0}.png", 종목코드);

                        await bot.SendMessage(조건검색식종목메세지);
                }
            }

            if (e.sRQName.Length > 8)
            {
                if (e.sRQName.Substring(0, 8).Equals("주식기본정보요청"))
                { 
                    String[] str = e.sRQName.Split(';');
                    if (str.Length == 2)
                    {
                        String 편입알림메세지 = "[조건식 편입 알림]\n";
                        String 조건명 = str[1];
                        String 종목코드 = api.GetCommData(e.sTrCode, e.sRQName, 0, "종목코드").Trim();
                        String 종목명 = api.GetCommData(e.sTrCode, e.sRQName, 0, "종목명").Trim();
                        String 포착가 = String.Format("{0:#,###}", int.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "현재가").Trim()));
                        String 거래량 = String.Format("{0:#,###}", int.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "거래량").Trim()));
                        String 거래대금 = String.Format("{0:#,###}", int.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "거래대금").Trim()));

                        편입알림메세지 += String.Format("{0} ({1}) 편입\n", 종목명, 종목코드);
                        편입알림메세지 += String.Format("검색 조건 : {0}\n", 조건명); 
                        편입알림메세지 += String.Format("편입 시간 : {0}\n", TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Korea Standard Time").ToString()); 
                        편입알림메세지 += String.Format("포착가 : {0}\n", 포착가); 
                        편입알림메세지 += String.Format("거래량 : {0}\n", 거래량);
                        편입알림메세지 += String.Format("거래대금 : {0}\n", 거래대금);
                        편입알림메세지 += String.Format("https://ssl.pstatic.net/imgfinance/chart/item/area/day/{0}.png", 종목코드);
                        await bot.SendMessage(편입알림메세지);
                    }
                }
            }
        }

        private void onReceiveTrCondition(object sender, _DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            if (e.strCodeList.Length == 0)
            {
                return;
            }

            var datas = 조건검색종목.CreateInstances(api, e);
            string stockCodeList = e.strCodeList.Remove(e.strCodeList.Length - 1);
            int stockCount = stockCodeList.Split(';').Length;

            Console.WriteLine(stockCodeList);
            if (stockCount <= 100)
            {
                api.CommKwRqData(stockCodeList, 0, stockCount, 0, "조건검색종목", GetScrNum());
            }

            if (e.nNext == 2)//연속조회여부 , 100개 이상 더 종목이 있을때 한번 더 조건검색을 요청한다.
            {
                api.SendCondition(e.sScrNo, e.strConditionName, e.nIndex, 2);//
            }
        }

        private async void onReceiveConditionVer(object sender, _DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveConditionVer", e.lRet));
            if (e.lRet != 1)
            {
                return;
            }

            조건식목록 = 조건식.CreateInstances(api, e).ToList();

            String 조건식목록string = "[조건식 목록]\n";

            foreach (var 조건식 in 조건식목록)
            {
                조건식목록string += "("+조건식.번호+")";
                조건식목록string += "   "+조건식.이름;
                조건식목록string += '\n';
                // api.SendCondition("5101", 조건식.이름, 조건식.번호, 조건식.실시간등록여부? 1:0);
            }
            await bot.SendMessage(조건식목록string);
        }


        private void onReceiveRealCondition(object sender, _DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveRealCondition", e.strConditionName));
            var data = 실시간_조건변경.CreateInstance(api, e);
            if (e.strType == "I")
            {
                api.SetInputValue("종목코드", data.종목코드);
                api.CommRqData("주식기본정보요청;" + data.조건이름, "opt10001", 0, GetScrNum());
            }
        }

        private void onReceiveRealData(object sender, _DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            
            //if(e.sRealType != "주식체결" && e.sRealType != "주식호가잔량" && e.sRealType != "주식우선호가" && e.sRealType != "종목프로그램매매" && e.sRealType != "주식예상체결" )
           // {
             //   Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveRealData", e.sRealType));
           //     Console.WriteLine(e.sRealData);
               // Console.WriteLine(e.sRealKey);
            //}
        }

        private async void onEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            if (e.nErrCode != 0)
            {
                await bot.SendMessage("로그인 실패ㅠㅜ\n다시 로그인 해주세요!");
                return;
            }
            await bot.SendMessage("로그인 성공!");
            await bot.SendMessage(String.Format("안녕하세요! {0}님,\n로그인에 성공하였습니다.\n알림이 꺼져있다면, '/알림시작' 해주세요.", 유저이름));

        }
        public void Logout()
        {

        }

        public async void StartJogun(int index)
        {
            if (ConnectState() == 0)
            {
                await bot.SendMessage("로그인이 필요합니다.");
                return;
            }
            String 특정조건식감시시작string = "[조건식 감시시작 알림]\n";

            if (조건식목록 == null)
            {
                await bot.SendMessage("'/조건식리스트'를 통해 조건식을 불러오지 않았습니다.\n조건식을 불러옵니다...\n원하는 조건식을 다시 실행해주세요.");
                api.GetConditionLoad();
            }

            if(조건식목록 != null) 
            {

                foreach (var 조건식 in 조건식목록)
                {
                    if (조건식.번호 == index)
                    {
                        api.SendCondition(GetScrNum(), 조건식.이름, 조건식.번호, 1);
                        조건식.실시간등록여부 = true;

                        특정조건식감시시작string += "조건 번호 : " + 조건식.번호 + "\n";
                        특정조건식감시시작string += "조건 검색식 : " + 조건식.이름 + "\n";
                        특정조건식감시시작string += "감시를 시작합니다.";

                       await bot.SendMessage(특정조건식감시시작string);
                    }
                }
            }
           
        }

        public async void StopJogun(int index)
        {
            if (ConnectState() == 0)
            {
                await bot.SendMessage("로그인이 필요합니다.");
                return;
            }

            String 특정조건식감시중단string = "[조건식 감시중단 알림]\n";

            if (조건식목록 == null)
            {
                await bot.SendMessage("'/조건식리스트'를 통해 조건식을 불러오지 않았습니다.\n조건식을 불러옵니다..\n원하는 조건식을 다시 중단해주세요.");
                api.GetConditionLoad();
            }

            if (조건식목록 != null) 
            {
                foreach (var 조건식 in 조건식목록)
                {
                    if (조건식.번호 == index)
                    {
                        api.SendConditionStop(GetScrNum(), 조건식.이름, 조건식.번호);
                        조건식.실시간등록여부 = false;

                        특정조건식감시중단string += "조건 번호 : " + 조건식.번호 + "\n";
                        특정조건식감시중단string += "조건 검색식 : " + 조건식.이름 + "\n";
                        특정조건식감시중단string += "감시를 중단합니다.";

                        await bot.SendMessage(특정조건식감시중단string);
                    }
                }
            }
        }

        public async void GetWatchJogun()
        {

            if (ConnectState() == 0)
            {
                await bot.SendMessage("로그인이 필요합니다.");
                return;
            }

            string watchCount = "[현재 감시중인 조건식 리스트]\n";

            if (조건식목록 == null)
            {
                watchCount = "현재 감시중인 조건식이 없습니다.";
                await bot.SendMessage(watchCount);
            }

            else if (조건식목록 != null) 
            {

                foreach (var 조건식 in 조건식목록)
                {
                    if (조건식.실시간등록여부 == true)
                    {
                        watchCount += "조건식 번호 : " + 조건식.번호 + ",  조건식 이름 : " + 조건식.이름;
                    }
                }

                if(watchCount == "[현재 감시중인 조건식 리스트]\n")
                {
                    watchCount = "현재 감시중인 조건식이 없습니다.";
                }

                await bot.SendMessage(watchCount);
            }
            
        }

        public int ConnectState()
        {
            return api.GetConnectState();
        }

        public async void GetUserJogun()
        {
            if (ConnectState() == 0)
            {
                await bot.SendMessage("로그인이 필요합니다.");
                return;
            }

            api.GetConditionLoad();
        }


        public void CommConnect()
        {
            api.CommConnect();
            유저이름 = api.GetLoginInfo("USER_NAME");
        }
    }
}

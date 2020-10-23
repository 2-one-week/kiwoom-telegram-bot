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

        private String telegramToken;

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

        public void LinkWithTelegram(TelegramManager telegramManager)
        {
            bot = telegramManager;
        }

        private void onReceiveTrData(object sender, _DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveTrData", e.sRecordName));

            if (e.sRQName == "조건검색종목")
            {
                int count = api.GetRepeatCnt(e.sTrCode, e.sRQName);//요청의 반복 횟수를 요청합니다.

                for (int i = 0; i < count; i++)
                {
                    string stockCode = api.GetCommData(e.sTrCode, e.sRQName, i, "종목코드").Trim();
                    string stockName = api.GetCommData(e.sTrCode, e.sRQName, i, "종목명").Trim();
                    string currentPrice = String.Format("{0:#,0}", int.Parse(api.GetCommData(e.sTrCode, e.sRQName, i, "현재가").Trim().Replace("-", "").Replace("+", "")));
                    string netChange = String.Format("{0:#,#}", int.Parse(api.GetCommData(e.sTrCode, e.sRQName, i, "거래대금").Trim()));
                    string volume = String.Format("{0:#,#}", int.Parse( api.GetCommData(e.sTrCode, e.sRQName, i, "거래량").Trim()));
                    Console.WriteLine(String.Format("종목코드 : {0} 종목명 : {1}  현재가:{2} 거래대금 : {3} 거래량: {4}\n", stockCode, stockName, currentPrice, netChange, volume));              
                }
            }
            if (e.sRQName.Length > 8)
            {
                if (e.sRQName.Substring(0, 8).Equals("주식기본정보요청"))
                {

                    String[] str = e.sRQName.Split(';');
                    if (str.Length == 2)
                    {
                        String 조건명 = str[1];
                        String 종목코드 = api.GetCommData(e.sTrCode, e.sRQName, 0, "종목코드").Trim();
                        String 종목명 = api.GetCommData(e.sTrCode, e.sRQName, 0, "종목명").Trim();
                        String 포착가 = String.Format("{0:#,###}", api.GetCommData(e.sTrCode, e.sRQName, 0, "현재가").Trim());
                        String 거래량 = String.Format("{0:#,###}", api.GetCommData(e.sTrCode, e.sRQName, 0, "거래량").Trim());
                        String 거래대금 = String.Format("{0:#,###}", api.GetCommData(e.sTrCode, e.sRQName, 0, "거래대금").Trim());
                        Console.WriteLine(조건명, 종목코드, 종목명, 포착가, 거래량, 거래대금);
                        
                    }
                }
            }
        }

        private void onReceiveTrCondition(object sender, _DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveTrCondition", e.strConditionName));
            if (e.strCodeList.Length == 0)
            {
                return;
            }

            var datas = 조건검색종목.CreateInstances(api, e);
            string stockCodeList = e.strCodeList.Remove(e.strCodeList.Length - 1);
            int stockCount = stockCodeList.Split(';').Length;

            if (stockCount <= 100)
            {
                api.CommKwRqData(stockCodeList, 0, stockCount, 0, "조건검색종목", "5100");
            }

            if (e.nNext != 0)//연속조회여부 , 100개 이상 더 종목이 있을때 한번 더 조건검색을 요청한다.
            {
                api.SendCondition("5101", e.strConditionName, e.nIndex, 1);//
            }
        }

        private void onReceiveConditionVer(object sender, _DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveConditionVer", e.lRet));
            if (e.lRet != 1)
            {
                return;
            }

            조건식목록 = 조건식.CreateInstances(api, e).ToList();

            String 조건식목록string = String.Empty;

            Console.WriteLine(조건식목록);

            foreach (var 조건식 in 조건식목록)
            {
                Console.WriteLine(조건식.번호);
                조건식목록string += 조건식.이름;
                조건식목록string += 조건식.번호;
                조건식목록string += '\n';
               // api.SendCondition("5101", 조건식.이름, int.Parse(조건식.번호), 1);
                // api.SendCondition("5101", 조건식.이름, 조건식.번호, 조건식.실시간등록여부? 1:0);
            }
            
        }


        private void onReceiveRealCondition(object sender, _DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveRealCondition", e.strConditionName));
            var data = 실시간_조건변경.CreateInstance(api, e);
            if (e.strType == "I")
            {
                api.SetInputValue("종목코드", data.종목코드);
                api.CommRqData("주식기본정보요청;" + data.조건이름, "opt10001", 0, "5100");
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

        private void onEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            if (e.nErrCode != 0)
            {
                Console.WriteLine("login failed");
                return;
            }

            Console.WriteLine("login success");
        }

        public void GetUserJogun()
        {
            api.GetConditionLoad();
        }
        public async void CommConnect()
        {
            api.CommConnect();
            await bot.SendMessage("양방향참조?");
        }
    }
}

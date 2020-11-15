using AxKHOpenAPILib;
using Kiwoom.Models;
using Kiwoom.Network;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private static Dictionary<string, string> 조건식포착종목 = new Dictionary<string, string>();

        public KiwoomManager(AxKHOpenAPILib.AxKHOpenAPI api)
        {
            try
            {
                this.api = api;
                api.OnEventConnect += onEventConnect;
                api.OnReceiveTrData += onReceiveTrData;
                api.OnReceiveTrCondition += onReceiveTrCondition;
                api.OnReceiveRealData += onReceiveRealData;
                api.OnReceiveConditionVer += onReceiveConditionVer;
                api.OnReceiveRealCondition += onReceiveRealCondition;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
           
        }

        private string GetScrNum()
        {
            try
            {
                if (_scrNum < 9999)
                    _scrNum++;
                else
                    _scrNum = 5000;
                return _scrNum.ToString();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }

            return _scrNum.ToString();
        }


        public void LinkWithTelegram(TelegramManager telegramManager)
        {
            try { bot = telegramManager; }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
           
        }

        private async void onReceiveTrData(object sender, _DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            try {
                Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveTrData", e.sRQName));
                if (e.sRQName.Length > 8)
                {
                    if (e.sRQName.Substring(0, 6).Equals("조건검색종목"))
                    {
                        string 조건검색식종목메세지 = "[조건식 검색 종목 알림]\n\n";
                        String[] str = e.sRQName.Split(';');
                        int count = api.GetRepeatCnt(e.sTrCode, e.sRQName);//요청의 반복 횟수를 요청합니다.
                        string 조건명 = str[1];
                        for (int i = 0; i < count; i++)
                        {
                            
                            string 종목코드 = api.GetCommData(e.sTrCode, e.sRQName, i, "종목코드").Trim();
                            string 종목명 = api.GetCommData(e.sTrCode, e.sRQName, i, "종목명").Trim();
                            string 포착가 = String.Format("{0:#,#}", long.Parse(api.GetCommData(e.sTrCode, e.sRQName, i, "현재가").Trim().Replace("-", "").Replace("+", "")));
                            string 거래대금 = String.Format("{0:#,#}", double.Parse(api.GetCommData(e.sTrCode, e.sRQName, i, "거래대금").Trim()));
                            string 거래량 = String.Format("{0:#,#}", double.Parse(api.GetCommData(e.sTrCode, e.sRQName, i, "거래량").Trim()));
                            string 현재시간 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Korea Standard Time").ToString();

                            조건검색식종목메세지 += String.Format("[{0} ({1}) 검색됨]\n", 종목명, 종목코드);
                            조건검색식종목메세지 += String.Format("검색 조건 : {0}\n", 조건명);
                            조건검색식종목메세지 += String.Format("편입 시간 : {0}\n", 현재시간);
                            조건검색식종목메세지 += String.Format("포착가 : {0}\n", 포착가);
                            조건검색식종목메세지 += String.Format("거래량 : {0}\n", 거래량);
                            조건검색식종목메세지 += String.Format("거래대금 : {0}\n\n", 거래대금);

                            조건식포착종목[조건명 + 종목코드] = 현재시간;
                           
                        }
                       await bot.SendMessage(조건검색식종목메세지);
                    }

                    if (e.sRQName.Substring(0, 8).Equals("주식기본정보요청"))
                    {
                        String[] str = e.sRQName.Split(';');
                        if (str.Length == 2)
                        {
                            string 편입알림메세지 = "[조건식 편입 알림]\n";
                            string 조건명 = str[1];
                            string 종목코드 = api.GetCommData(e.sTrCode, e.sRQName, 0, "종목코드").Trim();
                            string 종목명 = api.GetCommData(e.sTrCode, e.sRQName, 0, "종목명").Trim();
                            string 포착가 = String.Format("{0:#,#}", long.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "현재가").Trim().Replace("-", "").Replace("+", "")));
                            string 거래량 = String.Format("{0:#,#}", double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "거래량").Trim()));
                            string 거래대금 = String.Format("{0:#,#}", double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "거래대금").Trim()));
                            string 현재시간 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Korea Standard Time").ToString();
                            

                            편입알림메세지 += String.Format("{0} ({1}) 편입\n", 종목명, 종목코드);
                            편입알림메세지 += String.Format("검색 조건 : {0}\n", 조건명);
                            편입알림메세지 += String.Format("편입 시간 : {0}\n", 현재시간);
                            편입알림메세지 += String.Format("포착가 : {0}원\n", 포착가);
                            편입알림메세지 += String.Format("거래량 : {0}\n", 거래량);
                            편입알림메세지 += String.Format("거래대금 : {0}\n", 거래대금);
                            편입알림메세지 += String.Format("https://t1.daumcdn.net/finance/chart/kr/daumstock/d/A{1}.png?t={2}", 종목명,종목코드, DateTime.Now.ToString("yyyyMMddhhmmss"));
                            //편입알림메세지 += String.Format("https://ssl.pstatic.net/imgfinance/chart/item/area/day/{0}.png", 종목코드);

                            Console.WriteLine("편입종목 알림 알림 알림 메세지 이게 안뜬다고?");
                            await bot.SendMessage(편입알림메세지);
                            조건식포착종목[조건명 + 종목코드] = 현재시간;
                        }
                    }

                    if (e.sRQName.Substring(0, 10).Equals("주식기본정보호가요청"))
                    {
                        String[] str = e.sRQName.Split(';');
                        if (str.Length == 2)
                        {
                            string 편입종목호가알림메세지 = "[조건식 편입 종목 호가 알림]\n";
                            string 조건명 = str[1];

                            string 종목코드 = api.GetCommData(e.sTrCode, e.sRQName, 0, "종목코드").Trim();
                            string 종목명 = api.GetCommData(e.sTrCode, e.sRQName, 0, "종목명").Trim();

                            string 현재시간 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Korea Standard Time").ToString();

                            편입종목호가알림메세지 += String.Format("{0} ({1})\n", 종목명, 종목코드);
                            편입종목호가알림메세지 += String.Format("검색 조건: {0}\n", 조건명);
                            편입종목호가알림메세지 += String.Format("검색 시간: {0}\n", 현재시간);

                            double 매도1호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도1호가잔량").Trim());
                            double 매도2호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도2호가잔량").Trim());
                            double 매도3호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도3호가잔량").Trim());
                            double 매도4호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도4호가잔량").Trim());
                            double 매도5호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도5호가잔량").Trim());
                            double 매도6호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도6호가잔량").Trim());
                            double 매도7호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도7호가잔량").Trim());
                            double 매도8호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도8호가잔량").Trim());
                            double 매도9호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도9호가잔량").Trim());
                            double 매도10호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매도10호가잔량").Trim());

                            double 매도잔량총합 = 매도1호가잔량 + 매도2호가잔량 + 매도3호가잔량 + 매도4호가잔량 + 매도5호가잔량 + 매도6호가잔량 + 매도7호가잔량 + 매도8호가잔량 + 매도9호가잔량 + 매도10호가잔량;

                            편입종목호가알림메세지 += String.Format("매도 잔량 총합 : {0:#,#}\n", 매도잔량총합);

                            double 매수1호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수1호가잔량").Trim());
                            double 매수2호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수2호가잔량").Trim());
                            double 매수3호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수3호가잔량").Trim());
                            double 매수4호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수4호가잔량").Trim());
                            double 매수5호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수5호가잔량").Trim());
                            double 매수6호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수6호가잔량").Trim());
                            double 매수7호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수7호가잔량").Trim());
                            double 매수8호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수8호가잔량").Trim());
                            double 매수9호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수9호가잔량").Trim());
                            double 매수10호가잔량 = double.Parse(api.GetCommData(e.sTrCode, e.sRQName, 0, "매수10호가잔량").Trim());

                            double 매수잔량총합 = 매수1호가잔량 + 매수2호가잔량 + 매수3호가잔량 + 매수4호가잔량 + 매수5호가잔량 + 매수6호가잔량 + 매수7호가잔량 + 매수8호가잔량 + 매수9호가잔량 + 매수10호가잔량;

                            편입종목호가알림메세지 += String.Format("매수 잔량 총합 : {0:#,#}\n", 매수잔량총합);

                            Console.WriteLine("편입종목 호가메세지보냄");
                            await bot.SendMessage(편입종목호가알림메세지);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }

        }



        private void onReceiveTrCondition(object sender, _DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            try {
                if (e.strCodeList.Length == 0)
                {
                    return;
                }
                var datas = 조건검색종목.CreateInstances(api, e);
                Console.WriteLine(String.Format("{0} 을 전체 검색", e.strCodeList));
                string stockCodeList = e.strCodeList.Remove(e.strCodeList.Length - 1);
                int stockCount = stockCodeList.Split(';').Length;

                if (stockCount <= 100)
                {
                    api.CommKwRqData(stockCodeList, 0, stockCount, 0, "조건검색종목;" + e.strConditionName, GetScrNum());
                }

                if (e.nNext == 2)//연속조회여부 , 100개 이상 더 종목이 있을때 한번 더 조건검색을 요청한다.
                {
                    api.SendCondition(e.sScrNo, e.strConditionName, e.nIndex, 1);//
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            
        }

        private void onReceiveConditionVer(object sender, _DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            try {
                Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveConditionVer", e.lRet));
                if (e.lRet != 1)
                {
                    return;
                }

                if(조건식목록 == null)
                {
                    조건식목록 = 조건식.CreateInstances(api, e).ToList();
                }
                
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }


        private void onReceiveRealCondition(object sender, _DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            try {
                var data = 실시간_조건변경.CreateInstance(api, e);
                if (e.strType == "I")
                {
                    if (조건식포착종목.ContainsKey(e.strConditionName + data.종목코드))
                    {
                      Console.WriteLine(String.Format("event={0} real-type={1} {2} 편입함, 근데 이미 종목 포함되어 있음", "onReceiveRealCondition", e.strConditionName , data.종목명));
                        Kiwoom.kiwoomform.log.Items.Add(String.Format("시간 : {0} 조건식명 : {1} 종목 :{2} 편입함, 근데 이미 종목 포함되어 있음", DateTime.Now.ToString(), e.strConditionName, data.종목명));
                        return;
                    }

                    조건식포착종목[e.strConditionName + data.종목코드] = "";

                    Console.WriteLine(String.Format("event={0} real-type={1} {2} 편입함", "onReceiveRealCondition", e.strConditionName, data.종목명));
                    Kiwoom.kiwoomform.log.Items.Add(String.Format("시간 : {0}조건식명 : {1} 종목 : {2} 편입함", DateTime.Now.ToString(), e.strConditionName, data.종목명));

                    api.CommKwRqData(data.종목코드, 0, 1, 0, "주식기본정보요청;" + e.strConditionName, GetScrNum());

                    Task.Delay(100);

                    foreach (var 조건식 in 조건식목록)
                    {
                        if (조건식.호가알림 == true && 조건식.이름 == e.strConditionName)
                        {
                            api.SetInputValue("종목코드", data.종목코드);
                            api.CommRqData("주식기본정보호가요청;" + e.strConditionName, "opt10007", 0, GetScrNum());
                        }
                    }

                    Task.Delay(200);
                }

                if (e.strType == "D")
                {
                    Console.WriteLine(String.Format("{0} 이탈함", data.종목명));
                    Kiwoom.kiwoomform.log.Items.Add(String.Format("시간: {0} 조건식명 : {1} 종목 :{2} 이탈함", DateTime.Now.ToString(),e.strConditionName, data.종목명));
                    return;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            
        }

        private void onReceiveRealData(object sender, _DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            //   Console.WriteLine(String.Format("event={0} real-type={1}", "onReceiveRealData", e.sRealType));
        }

        private async void onEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            try {
                if (e.nErrCode != 0)
                {
                    await bot.SendMessageToAdmin("로그인 실패ㅠㅜ\n다시 로그인 해주세요!");
                    return;
                }

                유저이름 = api.GetLoginInfo("USER_NAME");
                Kiwoom.kiwoomform.log.Items.Add(유저이름 + "님 로그인");

                await bot.SendMessageToAdmin(유저이름+"님 로그인 성공했습니다.\n'/연결상태'로 키움과의 연결상태를 확인하세요.");
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }

        }

        public async void StartJogun(long chatid, int index)
        {
            try {
                if (ConnectState() == 0)
                {
                    await bot.SendMessageToPeople(chatid,"로그인이 필요합니다.");
                    return;
                }
                
               String 특정조건식감시시작string = "[조건식 감시시작 알림]\n";

                if (조건식목록 == null)
                {
                    await bot.SendMessageToPeople(chatid,"'/조건식리스트'를 통해 조건식을 불러오지 않았습니다.\n조건식을 불러옵니다...\n원하는 조건식을 다시 실행해주세요.");
                    GetUserJogun(chatid);
                }

                if (조건식목록 != null)
                {

                    foreach (var 조건식 in 조건식목록)
                    {
                        if (조건식.번호 == index)
                        {
                            api.SendCondition(GetScrNum(), 조건식.이름, 조건식.번호, 1);

                            조건식.실시간등록여부 = true;
                            조건식.화면번호 = _scrNum.ToString();

                            Console.WriteLine("화면번호" + 조건식.화면번호 + "조건식 번호 : " + 조건식.번호 + ",  조건식 이름 : " + 조건식.이름);
                            특정조건식감시시작string += "조건 번호 : " + 조건식.번호 + "\n";
                            특정조건식감시시작string += "조건 검색식 : " + 조건식.이름 + "\n";
                            특정조건식감시시작string += "감시를 시작합니다.";

                            await bot.SendMessage(특정조건식감시시작string);
                            return;
                        }
                    }

                    await bot.SendMessageToPeople(chatid,String.Format("{0}에 해당하는 조건식은 존재하지 않습니다.", index));
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        public async void StopJogun(long chatid,int index)
        {
            try {
                if (ConnectState() == 0)
                {
                    await bot.SendMessageToPeople(chatid,"로그인이 필요합니다.");
                    return;
                }

                String 특정조건식감시중단string = "[조건식 감시중단 알림]\n";

                if (조건식목록 == null)
                {
                    await bot.SendMessageToPeople(chatid, "'/조건식리스트'를 통해 조건식을 불러오지 않았습니다.\n조건식을 불러옵니다..\n원하는 조건식을 다시 중단해주세요.");
                    GetUserJogun(chatid);
                }

                if (조건식목록 != null)
                {
                    foreach (var 조건식 in 조건식목록)
                    {
                        if (조건식.번호 == index)
                        {
                            Console.WriteLine("화면번호"+조건식.화면번호 + "조건식 번호 : " + 조건식.번호 + ",  조건식 이름 : " + 조건식.이름);

                            api.SendConditionStop(조건식.화면번호, 조건식.이름, 조건식.번호);
                            조건식.실시간등록여부 = false;

                            특정조건식감시중단string += "조건 번호 : " + 조건식.번호 + "\n";
                            특정조건식감시중단string += "조건 검색식 : " + 조건식.이름 + "\n";
                            특정조건식감시중단string += "감시를 중단합니다.";

                            await bot.SendMessage(특정조건식감시중단string);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }


        public async void StartHoga(long chatid, int index)
        {
            try
            {
                if (ConnectState() == 0)
                {
                    await bot.SendMessageToPeople(chatid, "로그인이 필요합니다.");
                    return;
                }

                String 특정조건식호가감시시작string = "[조건식 호가 감시 시작 알림]\n";

                if (조건식목록 == null)
                {
                    await bot.SendMessageToPeople(chatid, "'/조건식리스트'를 통해 조건식을 불러오지 않았습니다.\n조건식을 불러옵니다...\n원하는 조건식을 다시 실행해주세요.");
                    GetUserJogun(chatid);
                }

                if (조건식목록 != null)
                {

                    foreach (var 조건식 in 조건식목록)
                    {
                        if (조건식.번호 == index)
                        {
                            조건식.호가알림 = true;

                            Console.WriteLine("화면번호" + 조건식.화면번호 + "조건식 번호 : " + 조건식.번호 + ",  조건식 이름 : " + 조건식.이름);
                            특정조건식호가감시시작string += "조건 번호 : " + 조건식.번호 + "\n";
                            특정조건식호가감시시작string += "조건 검색식 : " + 조건식.이름 + "\n";
                            특정조건식호가감시시작string += "호가 알림을 시작합니다.";

                            await bot.SendMessage(특정조건식호가감시시작string);
                            return;
                        }
                    }

                    await bot.SendMessageToPeople(chatid, String.Format("{0}에 해당하는 조건식은 존재하지 않습니다.", index));
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        public async void StopHoga(long chatid, int index)
        {
            try
            {
                if (ConnectState() == 0)
                {
                    await bot.SendMessageToPeople(chatid, "로그인이 필요합니다.");
                    return;
                }

                String 특정조건식감시중단string = "[조건식 호가 감시 중단 알림]\n";

                if (조건식목록 == null)
                {
                    await bot.SendMessageToPeople(chatid, "'/조건식리스트'를 통해 조건식을 불러오지 않았습니다.\n조건식을 불러옵니다..\n원하는 조건식을 다시 중단해주세요.");
                    GetUserJogun(chatid);
                }

                if (조건식목록 != null)
                {
                    foreach (var 조건식 in 조건식목록)
                    {
                        if (조건식.번호 == index)
                        {
                            Console.WriteLine("화면번호" + 조건식.화면번호 + "조건식 번호 : " + 조건식.번호 + ",  조건식 이름 : " + 조건식.이름);

                            조건식.호가알림 = false;

                            특정조건식감시중단string += "조건 번호 : " + 조건식.번호 + "\n";
                            특정조건식감시중단string += "조건 검색식 : " + 조건식.이름 + "\n";
                            특정조건식감시중단string += "호가 알림을 중단합니다.";

                            await bot.SendMessage(특정조건식감시중단string);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        public async void GetWatchJogun(long id)
        {
            try {
                if (ConnectState() == 0)
                {
                    await bot.SendMessageToPeople(id, "로그인이 필요합니다.");
                    return;
                }

                string watchCount = "[현재 감시중인 조건식 리스트]\n";

                if (조건식목록 == null)
                {
                    watchCount = "현재 감시중인 조건식이 없습니다.";
                    await bot.SendMessageToPeople(id,watchCount);
                }

                else if (조건식목록 != null)
                {

                    foreach (var 조건식 in 조건식목록)
                    {
                        if (조건식.실시간등록여부 == true)
                        {
                            watchCount += "조건식 번호 : " + 조건식.번호 + ",  조건식 이름 : " + 조건식.이름 + "\n";
                        }
                    }

                    if (watchCount == "[현재 감시중인 조건식 리스트]\n")
                    {
                        watchCount = "현재 감시중인 조건식이 없습니다.";
                    }

                    await bot.SendMessageToPeople(id,watchCount);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        public int ConnectState()
        {
            try { return api.GetConnectState(); }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            return -1;
        }

        public async void GetUserJogun(long id)
        {
            try {
                if (ConnectState() == 0)
                {
                    await bot.SendMessageToPeople(id,"로그인이 필요합니다.");
                    return;
                }

                if(조건식목록 == null)
                {
                    api.GetConditionLoad();
                }
                
                Thread.Sleep(1000);

                String 조건식목록string = "[조건식 목록]\n";

                foreach (var 조건식 in 조건식목록)
                {
                    조건식목록string += "(" + 조건식.번호 + ")";
                    조건식목록string += " " + 조건식.이름;
                    if(조건식.실시간등록여부 == true && !조건식.호가알림)
                    {
                        조건식목록string += "--[실시간감시중]";

                    }else if(조건식.실시간등록여부 == true && 조건식.호가알림)
                    {
                        조건식목록string += "--[실시간감시중]--[호가알림]";
                    }
                    조건식목록string += '\n';
                }
                await bot.SendMessageToPeople(id, 조건식목록string);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }


        public void CommConnect()
        {
            try
            {
                api.CommConnect();
                
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }
    }
}
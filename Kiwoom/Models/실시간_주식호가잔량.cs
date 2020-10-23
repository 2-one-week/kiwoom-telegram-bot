using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Kiwoom.Models
{
    public class 실시간_주식호가잔량_호가
    {
        public string 호가 { get; set; }
        public string 호가수량 { get; set; }
        public string 호가직전대비 { get; set; }
    }
    public class 실시간_주식호가잔량 : IModel
    {
        public static string REAL_TYPE = "주식호가잔량";
        public static string SIO_EVENT = "실시간_주식호가잔량";

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        public string 종목명 { get; set; }
        public string 종목코드 { get; set; }
        public string 호가시간{ get; set; }
        public 실시간_주식호가잔량_호가[] 매도호가 { get; set; }
        public 실시간_주식호가잔량_호가[] 매수호가 { get; set; }


        public static 실시간_주식호가잔량 CreateInstance(AxKHOpenAPILib.AxKHOpenAPI api, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            var 매도호가목록 = new List<실시간_주식호가잔량_호가>();
            for (var i = 1; i <= 10; i++)
            {
                매도호가목록.Add(new 실시간_주식호가잔량_호가
                {
                    호가 = api.GetCommRealData(e.sRealKey, 40+i).Trim(),
                    호가수량 = api.GetCommRealData(e.sRealKey, 60+i).Trim(),
                    호가직전대비 = api.GetCommRealData(e.sRealKey, 80+i).Trim(),
                });
            }

            var 매수호가목록 = new List<실시간_주식호가잔량_호가>();
            for (var i = 1; i <= 10; i++)
            {
                매수호가목록.Add(new 실시간_주식호가잔량_호가
                {
                    호가 = api.GetCommRealData(e.sRealKey, 50+i).Trim(),
                    호가수량 = api.GetCommRealData(e.sRealKey, 70+i).Trim(),
                    호가직전대비 = api.GetCommRealData(e.sRealKey, 90+i).Trim(),
                });
            }

            var data = new 실시간_주식호가잔량();
            data.종목명 = api.GetMasterCodeName(e.sRealKey);
            data.종목코드 = e.sRealKey;
            data.호가시간 = api.GetCommRealData(e.sRealKey, 21).Trim();
            data.매도호가 = 매도호가목록.ToArray();
            data.매수호가 = 매수호가목록.ToArray();
            return data;
        }
    }
}

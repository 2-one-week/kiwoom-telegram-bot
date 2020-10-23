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
    public class 실시간_주식체결 : IModel
    {
        public static string REAL_TYPE = "주식체결";
        public static string SIO_EVENT = "실시간_주식체결";

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        public string 종목명 { get; set; }
        public string 종목코드 { get; set; }
        public string 체결시간{ get; set; }
        public string 현재가 { get; set; }
        public string 등락률 { get; set; }
        public string 거래량 { get; set; }
        public string 거래회전율 { get; set; }
        public string 체결강도 { get; set; }
        public string 상한가발생시간 { get; set; }
        public string 하한가발생시간 { get; set; }


        public static 실시간_주식체결 CreateInstance(AxKHOpenAPILib.AxKHOpenAPI api, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            var data = new 실시간_주식체결();
            data.종목명 = api.GetMasterCodeName(e.sRealKey);
            data.종목코드 = e.sRealKey;
            data.체결시간 = api.GetCommRealData(e.sRealKey, 20).Trim();
            data.현재가 = api.GetCommRealData(e.sRealKey, 10).Trim();
            data.등락률 = api.GetCommRealData(e.sRealKey, 12).Trim();
            data.거래량 = api.GetCommRealData(e.sRealKey, 15).Trim();
            data.거래회전율 = api.GetCommRealData(e.sRealKey, 31).Trim();
            data.체결강도 = api.GetCommRealData(e.sRealKey, 228).Trim();
            data.상한가발생시간 = api.GetCommRealData(e.sRealKey, 567).Trim();
            data.하한가발생시간 = api.GetCommRealData(e.sRealKey, 568).Trim();
            return data;
        }
    }
}

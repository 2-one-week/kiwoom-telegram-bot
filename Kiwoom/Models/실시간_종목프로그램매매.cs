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
    public class 실시간_종목프로그램매매 : IModel
    {
        public static string REAL_TYPE = "종목프로그램매매";
        public static string SIO_EVENT = "실시간_종목프로그램매매";

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        public string 종목코드 { get; set; }
        public string 체결시간 { get; set; }
        public string 현재가 { get; set; }
        public string 등락률 { get; set; }
        public string 매도수량 { get; set; }
        public string 매도금액 { get; set; }
        public string 매수수량 { get; set; }
        public string 매수금액 { get; set; }
        public string 순매수수량 { get; set; }
        public string 순매수금액{ get; set; }
        public string 순매수금액증감 { get; set; }


        public static 실시간_종목프로그램매매 CreateInstance(AxKHOpenAPILib.AxKHOpenAPI api, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            var data = new 실시간_종목프로그램매매();
            data.종목코드 = e.sRealKey;
            data.체결시간 = api.GetCommRealData(e.sRealKey, 20).Trim();
            data.현재가 = api.GetCommRealData(e.sRealKey, 10).Trim();
            data.등락률 = api.GetCommRealData(e.sRealKey, 12).Trim();
            data.매도수량 = api.GetCommRealData(e.sRealKey, 202).Trim();
            data.매도금액 = api.GetCommRealData(e.sRealKey, 204).Trim();
            data.매수수량 = api.GetCommRealData(e.sRealKey, 206).Trim();
            data.매수금액 = api.GetCommRealData(e.sRealKey, 208).Trim();
            data.순매수수량 = api.GetCommRealData(e.sRealKey, 210).Trim();
            data.순매수금액 = api.GetCommRealData(e.sRealKey, 212).Trim();
            data.순매수금액증감 = api.GetCommRealData(e.sRealKey, 213).Trim();
            return data;
        }
    }
}
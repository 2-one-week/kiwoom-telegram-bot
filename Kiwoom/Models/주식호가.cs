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
    public class 주식호가 : IModel
    {
        public static string TR_CODE = "opt10004";
        public static string SIO_EVENT = "응답_주식호가";

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        public string 종목명 { get; set; }
        public string 종목코드 { get; set; }
        public string 거래량 { get; set; }
        public string 포착가 { get; set; }
        public string 거래대금 { get; set; }



        public static 주식호가 CreateInstance(AxKHOpenAPILib.AxKHOpenAPI api, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            var data = new 주식호가();
            data.종목명 = api.GetMasterCodeName(e.sRQName.Split(';').ElementAt(1));
            data.종목코드 = e.sRQName.Split(';').ElementAt(1);
            data.거래량 = api.GetCommData(e.sTrCode, e.sRQName, 0, "거래량").Trim();
            data.포착가 = api.GetCommData(e.sTrCode, e.sRQName, 0, "현재가").Trim();
            data.거래대금 = api.GetCommData(e.sTrCode, e.sRQName, 0, "거래대금").Trim();

            return data;
        }
    }

    public class 주식호가_Request : IRequest
    {
        public static string EVENT_NAME = "요청_주식호가";
        public string[] Codes { get; set; }

    }
}

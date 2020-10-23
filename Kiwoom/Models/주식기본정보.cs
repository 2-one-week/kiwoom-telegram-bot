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
    public class 주식기본정보 : IModel
    {
        public static string TR_CODE = "opt10001";
        public static string SIO_EVENT = "응답_주식기본정보";

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        public string 종목코드 { get; set; }
        public string 종목명 { get; set; }
        public string 포착가 { get; set; }
        public string 거래대금 { get; set; }
        public string 거래량 { get; set; }


        public static 주식기본정보 CreateInstance(AxKHOpenAPILib.AxKHOpenAPI api, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            var data = new 주식기본정보();
            data.종목코드 = e.sRQName.Split(';').ElementAt(1);

            if (data.종목코드.IsNullOrEmpty())
            {
                data.Error = "없는 종목 코드";
                return data;
            }

            data.종목명 = api.GetMasterCodeName(e.sRQName.Split(';').ElementAt(1)).Trim();
            data.포착가 = String.Format("{0:#,###}",api.GetCommData(e.sTrCode, e.sRQName, 0, "현재가").Trim());
            data.거래대금 = String.Format("{0:#,###}", api.GetCommData(e.sTrCode, e.sRQName, 0, "거래대금").Trim());
            data.거래량 = String.Format("{0:#,###}", api.GetCommData(e.sTrCode, e.sRQName, 0, "거래량").Trim());
            return data;
        }
    }

    public class 주식기본정보_Request : IRequest
    {
        public static string EVENT_NAME = "요청_주식기본정보";
        public string Type { get; set; }
        public string[] Codes { get; set; }
    }
}

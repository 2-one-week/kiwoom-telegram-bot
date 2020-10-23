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
    public class 실시간_조건변경 : IModel
    {
        public static string REAL_TYPE = "조건변경";
        public static string SIO_EVENT = "실시간_조건변경";

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        public string 종목명 { get; set; }
        public string 종목코드 { get; set; }
        public string 조건이름 { get; set; }
        public string 조건변경 { get; set; }
        public string 현재가 { get; set; }

        public static 실시간_조건변경 CreateInstance(AxKHOpenAPILib.AxKHOpenAPI api, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            var data = new 실시간_조건변경();
            data.종목명 = api.GetMasterCodeName(e.sTrCode);
            data.종목코드 = e.sTrCode;
            data.조건이름 = e.strConditionName;
            data.조건변경 = (e.strType == "I") ? "편입" : "이탈";
            return data;
        }
    }
}

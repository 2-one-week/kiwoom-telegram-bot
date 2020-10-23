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
    public class 조건검색종목 : IModel
    {
        public static string TR_CODE = "조건검색종목";
        public static string SIO_EVENT = "응답_조건검색종목";

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
        public string 검색조건 { get; set; }
        public string 종목명 { get; set; }
        public string 종목코드 { get; set; }
        public static 조건검색종목[] CreateInstances(AxKHOpenAPILib.AxKHOpenAPI api, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            var dataList = new List<조건검색종목>();

            var codes = e.strCodeList.TrimEnd(';').Split(';');

            foreach (var code in codes)
            {
                var data = new 조건검색종목();
                data.검색조건 = e.strConditionName;
                data.종목명 = api.GetMasterCodeName(code);
                data.종목코드 = code;
                dataList.Add(data);
            }
            return dataList.ToArray();
        }
    }
}

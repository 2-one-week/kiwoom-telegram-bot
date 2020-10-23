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
    public class 조건식 : IModel
    {
        public static string SIO_EVENT = "응답_조건식";

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        public int 번호 { get; set; }
        public string 이름 { get; set; }

        public Boolean 실시간등록여부 = false;
        public double lasRequestTime;


        public static 조건식[] CreateInstances(AxKHOpenAPILib.AxKHOpenAPI api, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            var returnData = new List<조건식>();

            var conditions = api.GetConditionNameList().TrimEnd(';').Split(';');
            foreach (var condition in conditions)
            {
                var tokens = condition.Split('^');

                var data = new 조건식();
                data.번호 = int.Parse(tokens[0]);
                data.이름 = tokens[1];
                returnData.Add(data);
            }

            return returnData.ToArray();
        }
    }
    public class 조건식_Request : IRequest
    {
        public static string EVENT_NAME = "요청_조건식";
        public string Code { get; set; }

    }
}

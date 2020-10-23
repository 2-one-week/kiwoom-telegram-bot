using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kiwoom.Models
{
    public class 실시간_주식체결삭제_Request : IRequest
    {
        public static string EVENT_NAME = "요청_실시간_주식체결삭제";
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
    }
}

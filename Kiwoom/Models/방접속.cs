using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kiwoom.Models
{
    public class 방접속_Request : IRequest
    {
        public static string EVENT_NAME = "join";
        [JsonProperty(PropertyName = "room")]
        public string Room { get; set; }
    }
}

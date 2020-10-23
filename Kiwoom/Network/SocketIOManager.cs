using Kiwoom.Models;
using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.EventArguments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Kiwoom.Network
{
    public class SocketIOManager
    {
        private SocketIO client;
        public event EventHandler<ReceivedEventArgs> OnReceivedEvent;

        public SocketIOManager(string url)
        {
            client = new SocketIO(url);

            client.OnConnected += onConnected;
            client.OnReceivedEvent += onReceivedEvent;
        }

        public async Task Connect()
        {
            await client.ConnectAsync();
        }

        public async Task Send(string eventName, IModel data)
        {
            await client.EmitAsync(eventName, data);
        }

        public async Task Send(string eventName, IEnumerable<IModel> datas)
        {
            await client.EmitAsync(eventName, datas);
        }

        private void onConnected(object sender, EventArgs e)
        {
            client.EmitAsync(방접속_Request.EVENT_NAME, new 방접속_Request { Room = "bot" });                
            Console.WriteLine("connected");
        }

        private void onReceivedEvent(object sender, ReceivedEventArgs e)
        {
            Console.WriteLine(String.Format("event={0}", e.Event));
            if (e.Event == 방접속_Request.EVENT_NAME)
            {
                return;
            }

            if (OnReceivedEvent != null )
            {
                OnReceivedEvent(this, e);
            }
        }
    }
}

﻿using SocketIOClient.EventArguments;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


///텔레그램 dll using 
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace Kiwoom.Network
{ 
    public class TelegramManager
    {
        private TelegramBotClient bot;
        private KiwoomManager Kiwoom;
        private ChatId chatId;

        public TelegramManager(string accessToken, KiwoomManager kiwoom)
        {
            bot = new Telegram.Bot.TelegramBotClient(accessToken);

            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
        }

        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text) return;

            if (message.Text.StartsWith("/알림시작"))
            {
                chatId = message.Chat.Id;
                await SendMessage("실시간 알림을 시작합니다.");
            }

            if (message.Text.StartsWith("/알림중지"))
            {
                chatId = null;
                await SendMessage("실시간 알림을 중지합니다.");
            }
        }

        public async Task SendMessage(string data)
        {
            await bot.SendTextMessageAsync(chatId, data);
        }
    }
    
}

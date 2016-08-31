using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Bot.Connector;

namespace RT_Test.Controllers
{
    public class ToTestController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Ms(string msg)
        {
            string err = "no errors", tconv = "";
            try
            {
                var tuser = new ChannelAccount(name: "", id: "@255348756");
                var suser = new ChannelAccount(name: "", id: "@29:1t9q0pbwa_jOKKqwQgQZLfbzjY09mHBxIbjlYE0n299E");
                var fuser = new ChannelAccount(name: "", id: "@1235237286494788");

                var sbotAccount = new ChannelAccount(name: "258", id: @"28:13761c30-c534-4f37-b534-a75ea659ff17");
                var tbotAccount = new ChannelAccount(name: "258", id: "rttestbot");
                var fbotAccount = new ChannelAccount(name: "258", id: "665520473616715");

                ConnectorClient connectorTelegramm = new ConnectorClient(new Uri("https://telegram.botframework.com/"));
                ConnectorClient connectorSkype = new ConnectorClient(new Uri("https://skype.botframework.com"));
                ConnectorClient connectorFacebook = new ConnectorClient(new Uri("https://facebook.botframework.com"));

                var tconversationId = new ResourceResponse("255348756");//await connectorSkype.Conversations.CreateDirectConversationAsync(tbotAccount, tuser); 

                var sconversationId = new ResourceResponse("29:1t9q0pbwa_jOKKqwQgQZLfbzjY09mHBxIbjlYE0n299E");
                var fconversationId = new ResourceResponse("1235237286494788-665520473616715");
                tconv = tconversationId.Id;

                IMessageActivity tmessage = Activity.CreateMessageActivity();
                tmessage.From = tbotAccount;
                tmessage.Recipient = tuser;
                tmessage.Conversation = new ConversationAccount(id: tconv);
                tmessage.Text = "tele Hello ----- " + msg;
                tmessage.Locale = "en-Us";
                tmessage.Attachments = new List<Attachment>();
                tmessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                IMessageActivity smessage = Activity.CreateMessageActivity();
                smessage.From = sbotAccount;
                smessage.Recipient = suser;
                smessage.Conversation = new ConversationAccount(id: sconversationId.Id);
                smessage.Text = "skype Hello ----- " + msg;
                smessage.Locale = "en-Us";
                smessage.Attachments = new List<Attachment>();
                smessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                IMessageActivity fmessage = Activity.CreateMessageActivity();
                fmessage.From = fbotAccount;
                fmessage.Recipient = fuser;
                fmessage.Conversation = new ConversationAccount(id: fconversationId.Id);
                fmessage.Text = "facebook Hello ----- " + msg;
                fmessage.Locale = "en-Us";
                fmessage.Attachments = new List<Attachment>();
                fmessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                List<CardImage> cardImages1 = new List<CardImage>();
                List<CardImage> cardImages2 = new List<CardImage>();
                cardImages1.Add(new CardImage(url: "http://www.fullhdoboi.com/wallpapers/thumbs/7/mini_kartinka-surok-2515.jpg"));
                cardImages2.Add(new CardImage(url: "http://pozdravok.ru/cards/lyubov/skuchayu/ty-gde-ya-skuchayu-kartinka.gif"));
                List<CardAction> cardButtons = new List<CardAction>();
                CardAction plButton = new CardAction()
                {
                    Value = "че я то",
                    Type = "imBack",
                    Title = "ты че"
                };
                cardButtons.Add(plButton);
                HeroCard plCard1 = new HeroCard()
                {
                    Title = "Hero Card 1",
                    Subtitle = "Subtitle Subtitle Subtitle",
                    Images = cardImages1,
                    Buttons = cardButtons
                };
                HeroCard plCard2 = new HeroCard()
                {
                    Title = "Hero Card 2",
                    Subtitle = "Subtitle Subtitle Subtitle",
                    Images = cardImages2,
                    Buttons = cardButtons
                };

                Attachment plAttachment1 = plCard1.ToAttachment();
                Attachment plAttachment2 = plCard2.ToAttachment();

                tmessage.Attachments.Add(plAttachment1);
                //tmessage.Attachments.Add(plAttachment2);
                smessage.Attachments.Add(plAttachment1);
                smessage.Attachments.Add(plAttachment2);
                fmessage.Attachments.Add(plAttachment1);
                fmessage.Attachments.Add(plAttachment2);

                await connectorTelegramm.Conversations.SendToConversationAsync((Activity)tmessage);
                await connectorSkype.Conversations.SendToConversationAsync((Activity)smessage);
                await connectorFacebook.Conversations.SendToConversationAsync((Activity)fmessage);
            }
            catch (Exception ex)
            {
                err = ex.Message + " ------------------- "+ ex.StackTrace;
            }
            var response = Request.CreateResponse(HttpStatusCode.OK,err);
            return response;
        }
    }
}
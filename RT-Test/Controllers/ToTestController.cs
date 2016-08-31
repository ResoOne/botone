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

                var sbotAccount = new ChannelAccount(name: "258", id: @"28:13761c30-c534-4f37-b534-a75ea659ff17");
                var tbotAccount = new ChannelAccount(name: "258", id: "rttestbot");

                ConnectorClient connectorTelegramm = new ConnectorClient(new Uri("https://telegram.botframework.com/"));
                ConnectorClient connectorSkype = new ConnectorClient(new Uri("https://skype.botframework.com"));

                var tconversationId = new ResourceResponse("255348756");//await connectorSkype.Conversations.CreateDirectConversationAsync(tbotAccount, tuser); 
                tconv = tconversationId.Id.TrimStart('@');
                var sconversationId = new ResourceResponse("29:1t9q0pbwa_jOKKqwQgQZLfbzjY09mHBxIbjlYE0n299E");
                tconv = tconversationId.Id;

                IMessageActivity tmessage = Activity.CreateMessageActivity();
                tmessage.From = tbotAccount;
                tmessage.Recipient = tuser;
                tmessage.Conversation = new ConversationAccount(id: tconv);
                tmessage.Text = @"tele Hello ----- " + msg;
                tmessage.Locale = "en-Us";

                IMessageActivity smessage = Activity.CreateMessageActivity();
                smessage.From = sbotAccount;
                smessage.Recipient = suser;
                smessage.Conversation = new ConversationAccount(id: sconversationId.Id);
                smessage.Text = @"skype Hello ----- "+ msg;
                smessage.Locale = "en-Us";

                await connectorTelegramm.Conversations.SendToConversationAsync((Activity)tmessage);
                await connectorSkype.Conversations.SendToConversationAsync((Activity)smessage);
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            var response = Request.CreateResponse(HttpStatusCode.OK,err);
            return response;
        }
    }
}
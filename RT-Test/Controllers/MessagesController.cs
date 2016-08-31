using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace RT_Test
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                string err = "", tconv = "";
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
                    tmessage.Text = "tele Hello";
                    tmessage.Locale = "en-Us";

                    IMessageActivity smessage = Activity.CreateMessageActivity();
                    smessage.From = sbotAccount;
                    smessage.Recipient = suser;
                    smessage.Conversation = new ConversationAccount(id: sconversationId.Id);
                    smessage.Text = "skype Hello";
                    smessage.Locale = "en-Us";

                    await connectorTelegramm.Conversations.SendToConversationAsync((Activity)tmessage);
                    await connectorSkype.Conversations.SendToConversationAsync((Activity)smessage);
                }
                catch(Exception ex)
                {
                    err = ex.Message;
                }
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;
                // return our reply to the user
                
                Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                //if (activity.Text == "p")
                //{
                    //reply.Text = "![duck](http://aka.ms/Fo983c)";
                //}
                reply.Text = JsonConvert.SerializeObject(activity);
                reply.Text += " @@@ tconv = " + tconv + " @@@";
                reply.Text += " ------------------------------------------" + err;


                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
            
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
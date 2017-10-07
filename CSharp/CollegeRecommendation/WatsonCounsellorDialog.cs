using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using CollegeRecommendation.Dialogs;

namespace CollegeRecommendation
{
    [Serializable]
    public class WatsonCounsellorDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {

            var message = context.MakeMessage();
            var attachment = counsellorimagecard();
            message.Attachments.Add(attachment);
            context.PostAsync(message);


            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
            
        {




            var activity = await result as Activity;

            WatsonApiCoun api = new WatsonApiCoun();

            String msg = activity.Text;
            String res = await api.TalkToWatson(msg);


            // WatsonConversation wc = new WatsonConversation();
            // String wat = activity.Text;
            //  var output = wc.Conversation(wat);
            var image = res.ToString();
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            //if (image == "hi_response") 
            //{
            //    await context.PostAsync("jaa be jhaatu");
            //    reply.Attachments = GetApiCardsListAttachments();

            //}
            //else
            //{F:\project work\Watson\watson\watson\Controllers\
            //    await context.PostAsync(res.ToString());
            //}
            if (image.Contains("growth_computers"))
            {
                reply.Attachments = growth_comps();
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }



            else

             if (image.Contains("growth_civil"))
            {
                reply.Attachments = growth_civil();
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }


            else

              if (image.Contains("growth_mechanical"))
            {
                reply.Attachments = growth_mech();
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }

            else

              if (image.Contains("growth_infotech"))
            {
                reply.Attachments = growth_it();
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }

            else

              if (image.Contains("high_it"))
            {
                reply.Attachments = high_it();
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }
            else

              if (image.Contains("high_comps"))
            {
                reply.Attachments = high_comps();
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }

            else

              if (image.Contains("high_civil"))
            {
                reply.Attachments = high_civil();
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }

            else

              if (image.Contains("high_mech"))
            {
                reply.Attachments = high_mech();
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);

            }

            else

            if (image.Contains("exit"))
            {
                await context.PostAsync("ok Bye!!!!!");
                context.Call(new RootDialog(), this.ExitAsync);
            }

            else
            {
                await context.PostAsync(res.ToString());
                context.Wait(MessageReceivedAsync);

            }
        }

        private async Task ExitAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Thanks for using our Counsellor service");
        }
        private static IList<Attachment> growth_comps()
        {

            return new List<Attachment>()
            {
                  GetcompsHeroCard(
                      "Growth in computer engineering"

                 )

           };
        }

        private static Attachment counsellorimagecard()
        {
            var counsellor = new HeroCard
            {
                Text = "",

                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-1.amazonaws.com/mvpar/counsellor.png") },

            };

            return counsellor.ToAttachment();
        }
        private static Attachment GetcompsHeroCard(string title)
        {
            var compsCard = new HeroCard
            {
                Title = title,
                Text = "Employment of computer hardware engineers is projected to grow 15% from 2012 to 2024, slower than the average for all occupations. A limited number of engineers will be needed to meet the demand for new computer hardware because more technological innovation takes place with software than with hardware. However, demand may grow for hardware engineers as more industries outside of the computer and electronic product manufacturing industry begin to research and develop their own electronic devices. Thus, although declining employment in the manufacturing industries that employ many of these workers will impede the growth of this occupation, computer hardware engineers should be less affected than production occupations because firms are less likely to outsource their type of work.",
                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-1.amazonaws.com/mvpar/growth_comps.png") }

            };

            return compsCard.ToAttachment();
        }



        private static IList<Attachment> growth_civil()
        {

            return new List<Attachment>()
            {
                  GetcivilHeroCard(
                      "Growth in civil engineering"

                 )

           };
        }
        private static Attachment GetcivilHeroCard(string title)
        {
            var civilCard = new HeroCard
            {
                Title = title,
                Text = "Employment of civil engineers is projected to grow 8 percent from 2014 to 2024, about as fast as the average for all occupations. As infrastructure continues to age, civil engineers will be needed to manage projects to rebuild bridges, repair roads, and upgrade levees and dams as well as airports and buildings.",
                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-1.amazonaws.com/mvpar/growth_civil.jpg") }

            };

            return civilCard.ToAttachment();
        }



        private static IList<Attachment> growth_mech()
        {

            return new List<Attachment>()
            {
                  GetmechHeroCard(
                      "Growth in mechanical engineering"

                 )

           };
        }
        private static Attachment GetmechHeroCard(string title)
        {
            var mechCard = new HeroCard
            {
                Title = title,
                Text = "Employment of mechanical engineers is projected to grow 23%  from 2012 to 2024, about as fast as the average for all occupations. Mechanical engineers can work in many industries and on many types of projects. As a result, their growth rate will differ by the industries that employ them.",
                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-1.amazonaws.com/mvpar/growth_mech.png") }

            };

            return mechCard.ToAttachment();
        }


        private static IList<Attachment> growth_it()
        {

            return new List<Attachment>()
            {
                  GetitHeroCard(
                      "Growth in Information technology"

                 )

           };
        }
        private static Attachment GetitHeroCard(string title)
        {
            var itCard = new HeroCard
            {
                Title = title,
                Text = "Employment and output in the computer systems design and related services industry is expected to grow rapidly as firms and individual consumers continue to increase their use of information technology services. Cloud computing is one area that is expected to contribute to growth in this industry",
                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-1.amazonaws.com/mvpar/growth_it.jpg") }

            };

            return itCard.ToAttachment();
        }

        private static IList<Attachment> high_it()
        {

            return new List<Attachment>()
            {
                  Gethigh_itHeroCard(
                      "Highly paid jobs in Information technology"

                 )

           };
        }
        private static Attachment Gethigh_itHeroCard(string title)
        {
            var highitCard = new HeroCard
            {
                Title = title,
                Text = "Enterprise Architect: $112,565 \r\r Applications Development Manager:$112,045 \r\r Software Engineering Manager:$109,350, \r\r IT Architect:$105,303, \r\r Solutions Architect: $102,678, \r\r Data Architect: $102,091, \r\r IT Program Manager: $98,883,  \r\r  UX Manager: $98,353, \r\r Systems Architect: $97,873, \r\r Scrum Master: $95,167, \r\r Development Engineer: $94,603, \r\r Data Scientist: $94,530, \r\r Analytics Manager: $93,597, \r\r Performance Engineer: $92,142",

                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-1.amazonaws.com/mvpar/high_it.jpg") }

            };

            return highitCard.ToAttachment();
        }


        private static IList<Attachment> high_comps()
        {

            return new List<Attachment>()
            {
                  Gethigh_compsHeroCard(
                      "Highly paid jobs in Computer engineering"

                 )

           };
        }
        private static Attachment Gethigh_compsHeroCard(string title)
        {
            var highcompsCard = new HeroCard
            {
                Title = title,
                Text = "Software engineer: $93,565 \r\r System engineer:$91,045 \r\r Software developer:$89,350, \r\r Java developer:$98,303, \r\r Buisness analyst: $84,678, \r\r .net developer: $91,091, \r\r Web developer: $82,883,  \r\r  System admin: $77,353, \r\r Project manager: $93,873, \r\r Network engineer: $91,167",

                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-1.amazonaws.com/mvpar/high_comps.jpg") }

            };

            return highcompsCard.ToAttachment();
        }

        private static IList<Attachment> high_civil()
        {

            return new List<Attachment>()
            {
                  Gethigh_civilHeroCard(
                      "Highly paid jobs in Civil engineering"

                 )

           };
        }
        private static Attachment Gethigh_civilHeroCard(string title)
        {
            var highcivilCard = new HeroCard
            {
                Title = title,
                Text = "Project manager:$58,560 \r\r Enigneering manager:$96,500 \r\r Civil engineer:$70,112 \r\r Architect:$88,440",

                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-1.amazonaws.com/mvpar/high_civil.jpg") }

            };

            return highcivilCard.ToAttachment();
        }

        private static IList<Attachment> high_mech()
        {

            return new List<Attachment>()
            {
                  Gethigh_mechHeroCard(
                      "Highly paid jobs in Mechanical engineering"

                 )

           };
        }
        private static Attachment Gethigh_mechHeroCard(string title)
        {
            var highmechCard = new HeroCard
            {
                Title = title,
                Text = "Marine engineer$99,000 \r\r Material engineer:$90,000 \r\r Mechanical engineer:$56,000 \r\r Aerospace Engineer:$85,000",

                Images = new List<CardImage> { new CardImage("https://s3-ap-southeast-1.amazonaws.com/mvpar/high_mech.jpg") }

            };

            return highmechCard.ToAttachment();
        }


        public class ConversationHelper
        {
            private readonly string _Server;
            private readonly NetworkCredential _NetCredential;
            public String response;
            //string workSpaceID = 9601472d-37df-4efa-9843-40d85709c361;
            public ConversationHelper(string workSpaceId, string userId, string password)
            {
                _Server = string.Format("https://gateway.watsonplatform.net/conversation/api/v1/workspaces/{0}/message?version={1}", workSpaceId, DateTime.Today.ToString("yyyy-MM-dd"));
                _NetCredential = new NetworkCredential(userId, password);
            }
            public async Task<string> GetResponse(string input, string context = null)
            {
                string req = null;
                if (string.IsNullOrEmpty(context)) req = "{\"input\": {\"text\": \"" + input + "\"}, \"alternate_intents\": true}";
                else req = "{\"input\": {\"text\": \"" + input + "\"}, \"alternate_intents\": true}, \"context\": \"" + context + "\"";
                using (var handler = new HttpClientHandler
                {
                    Credentials = _NetCredential
                })
                using (var client = new HttpClient(handler))
                {
                    var cont = new HttpRequestMessage();
                    cont.Content = new StringContent(req.ToString(), Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(_Server, cont.Content);
                    return await result.Content.ReadAsStringAsync();
                }
            }



        }

        public class WatsonConversation
        {

            public async Task<String> Conversation(String input)
            {
                var baseurl = "https://gateway.watsonplatform.net/conversation/api";
                var workspace = "2c417ac2-9494-4dc5-b84a-2fdaae39ffbb";
                var username = "9601472d-37df-4efa-9843-40d85709c361";
                var password = "uRZD7MMYPq55";
                var context = null as object;
                //var inputCon = input;
                var message = new { input = new { text = input }, context };

                var resp = await baseurl

                    .AppendPathSegments("v1", "workspaces", workspace, "message")
                    .SetQueryParam("version", "2016-11-21")
                    .WithBasicAuth(username, password)
                    .AllowAnyHttpStatus()
                    .PostJsonAsync(message);

                var json = await resp.Content.ReadAsStringAsync();

                var answer = new
                {
                    intents = default(object),
                    entities = default(object),
                    input = default(object),
                    output = new
                    {
                        text = default(string[])
                    },
                    context = default(object)
                };

                answer = JsonConvert.DeserializeAnonymousType(json, answer);

                var output = answer?.output?.text?.Aggregate(
                    new StringBuilder(),
                    (sb, l) => sb.AppendLine(l),
                    sb => sb.ToString());
                return output;
            }

        }
    }
}
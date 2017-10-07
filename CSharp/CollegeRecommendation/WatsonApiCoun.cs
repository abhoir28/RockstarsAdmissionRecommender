using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CollegeRecommendation
{
    public class WatsonApiCoun
    {

        public async Task<String> TalkToWatson(String msg)
        {

            var baseurl = "https://gateway.watsonplatform.net/conversation/api";
            var workspace = "2c417ac2-9494-4dc5-b84a-2fdaae39ffbb";
            var username = "9601472d-37df-4efa-9843-40d85709c361";
            var password = "uRZD7MMYPq55";
            var context = null as object;
            var input = msg;
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

            //Console.ForegroundColor = ConsoleColor.White;
            //// Console.WriteLine(output);
            // Console.ReadLine();
            //Console.ForegroundColor = ConsoleColor.Gray;
            //Console.WriteLine(json);
            //Console.ReadLine();
            //Console.ResetColor();

            return output;
        }

    }
}
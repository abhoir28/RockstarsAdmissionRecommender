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
    public class Watson
    {
        
        public async Task<String> TalkToWatson(String msg)
        {
            //Essential connections strings required for connecting to watson Conversation
            var baseurl = "https://gateway.watsonplatform.net/conversation/api";
            var workspace = "63dc3811-5b4d-48a7-81d6-4a41a703cc40";
            var username = "9601472d-37df-4efa-9843-40d85709c361";
            var password = "uRZD7MMYPq55";
            var context = null as object;
            var input = msg;
            var message = new { input = new { text = input }, context };

            //Sends the input to Watson Conversation and waits for the response 
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

            //Converts the Json obtained to the type defined for answer 
            answer = JsonConvert.DeserializeAnonymousType(json, answer);

            //Extracts only the required data which is to be displayed to the user
            var output = answer?.output?.text?.Aggregate(
                new StringBuilder(),
                (sb, l) => sb.AppendLine(l),
                sb => sb.ToString());

            return output;
        }
    }
}
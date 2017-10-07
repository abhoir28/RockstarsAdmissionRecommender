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
    public class WatsonCounsellorMarathiDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)

        {
            var activity = await result as Activity;

            WatsonApiCoun api = new WatsonApiCoun();

            String msg = activity.Text;
            String res = await api.TalkToWatson(msg);
            res = res.Replace("\r\n", string.Empty);
            res = res.Replace(".", string.Empty);
            String ResultTranslate = res;
            String langpair = "en|mr";
            String TranslatedResult = TranslateText(ResultTranslate, langpair);
            await context.PostAsync(TranslatedResult);
        }

        public static string TranslateText(string input, string languagePair)

        {
            string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", input, languagePair);
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            string result = webClient.DownloadString(url);
            result = result.Substring(result.IndexOf("<span title=\"") + "<span title=\"".Length);
            result = result.Substring(result.IndexOf(">") + 1);
            result = result.Substring(0, result.IndexOf("</span>"));
            return result;
        }
    }
}
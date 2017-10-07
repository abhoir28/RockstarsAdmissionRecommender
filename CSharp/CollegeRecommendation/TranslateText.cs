using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using System.Net;

namespace CollegeRecommendation
{
    [Serializable]
    public class TranslateText : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
           await context.PostAsync("Enter Text & it will be converted to Marathi");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            String input = activity.Text;
            String langpair = "en|mr";
            String TranslatedResult = TranslateTxt(input, langpair);
            await context.PostAsync(TranslatedResult);
            context.Wait(MessageReceivedAsync);
        }

        public static string TranslateTxt(string input, string languagePair)

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
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
    public class FrequentlyAskedQuestion : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi ! Let me get your doubts clear");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            //Creates an object of class Watson
            Watson api = new Watson();
            //Stores the user input in the msg variable
            String msg = activity.Text;

            //Calls the function of Watson class and passes the input of the user 
            //Waits for response class Watson
            String res = await api.TalkToWatson(msg);
            
            if (res.Contains("exit"))
            {
                await context.PostAsync("Alright!...Please Dont hesitate to contact me if I can be of further assistance.");
                context.Call(new RootDialog(), this.ExitAsync);
            }
            else
            {
                await context.PostAsync(res);
                context.Wait(MessageReceivedAsync);
            }
        }
        private async Task ExitAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("ok Bye!!!!!");
        }
    }
    

}
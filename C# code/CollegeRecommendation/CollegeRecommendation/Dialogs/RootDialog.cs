using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace CollegeRecommendation.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        
        public Task StartAsync(IDialogContext context)
        {
            //Calls and Waits for MessageReceievedAsync
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync("Hi I am a Rockstar Admission Counseller Bot.");
            //Calls MainOptions for Displaying Option
            this.MainOptions(context);
        }

        private const string FAQ = "Frequently Asked Questions";
        private const string Test = "Testomeny";
        private const string CollegeRecommenderOption = "CollegeRecommender";

        private void MainOptions(IDialogContext context)
        {
            //For Displaying buttons as Option
            PromptDialog.Choice(context, this.OnMainOptionSelected1, new List<string>() { CollegeRecommenderOption, FAQ, Test }, "Select any one", "Not a valid option", 3);
        }

        //Calling the respective class on the basis of option selected 
        private async Task OnMainOptionSelected1(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case CollegeRecommenderOption:
                    //Calls the DetailsDialog Class and treats it as a fucntion
                    context.Call(new DetailsDialog(), this.MessageReceivedAsync);
                    break;

                case FAQ:
                    //context.Call(new DetailsDialog(), this.MessageReceivedAsync);
                    break;

                case Test:
                    //context.Call(new DetailsDialog(), this.MessageReceivedAsync);
                    break;
            }
        }

    }
}
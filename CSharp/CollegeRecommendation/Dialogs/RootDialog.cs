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
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync("Hi I am a Rockstar Admission Counseller Bot.");
            this.MainOptions(context);
        }
        private const string FAQ = "Frequently Asked Questions";
        private const string Test = "Testomeny";
        private const string CollegeRecommenderOption = "CollegeRecommender";

        private void MainOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnMainOptionSelected1, new List<string>() { CollegeRecommenderOption, FAQ, Test }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnMainOptionSelected1(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case CollegeRecommenderOption:
                    //Calls the DetailsDialog and treats it as a function
                    context.Call(new DetailsDialog(), this.MessageReceivedAsync);
                    break;

                case FAQ:
                    //Calls the FrequentlyAskedQuestion and treats it as a function
                    context.Call(new FrequentlyAskedQuestion(), this.MessageReceivedAsync);
                    break;

                case Test:
                    //Calls the TextEmotion and treats it as a function
                    context.Call(new TextEmotion(), this.MessageReceivedAsync);
                    break;
            }
        }

    }
}
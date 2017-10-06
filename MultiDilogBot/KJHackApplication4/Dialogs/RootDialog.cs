using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace KJHackApplication4.Dialogs
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

            await context.PostAsync("Hi I am a RockStar Bot.");

            context.Call(new DetailsDialog(), this.Option);
        }

        private const string FAQ = "Genral Category";
        private const string CollegeRecommender = "College Recommender";
        private const string Testemony = "Testemony";
        private void OptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnCategoryOptionSelected, new List<string>() { FAQ, CollegeRecommender, Testemony }, "Select any one", "Not a valid option", 3);
        }

        private async Task OnCategoryOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case FAQ:

                    break;

                case CollegeRecommender:
                    context.Call(new DetailsDialog(), this.Option);
                    break;

                case Testemony:
                    
                    break;
            }

        }

        private async Task Option(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync("Hi i am a rockstar bot.");
            this.OptionSelected(context);
        }

    }
}
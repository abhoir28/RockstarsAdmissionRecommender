using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using Testing.Dialogs;

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
            var replyImage = context.MakeMessage();
            replyImage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            replyImage.Attachments = GetImagecardsAttachments();
            await context.PostAsync(replyImage);

        }
        private const string AdmissionProcedure = "Admission Procedure";
        private const string FAQ = "Frequently Asked Questions";
        private const string Counsellor = "Admission Counseller";
        private const string Test = "Testimony";
        private const string CollegeRecommenderOption = "CollegeRecommender";

        private void MainOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnMainOptionSelected1, new List<string>() {  CollegeRecommenderOption, AdmissionProcedure, Counsellor,  FAQ, Test }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnMainOptionSelected1(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case AdmissionProcedure:
                    //Calls the DetailsDialog and treats it as a function
                    context.Call(new AdmissionProcedureDialog(), this.MessageReceivedAsync);
                    break;

                case Counsellor:
                    //Calls the DetailsDialog and treats it as a function
                    this.CounsellerOptions(context);
                    break;

                case CollegeRecommenderOption:
                    //Calls the DetailsDialog and treats it as a function
                    context.Call(new DetailsDialog(), this.MessageReceivedAsync);
                    break;

                case FAQ:
                    //Calls the FrequentlyAskedQuestion and treats it as a function
                    context.Call(new FrequentlyAskedQuestion(), this.MessageReceivedAsync);
                    break;

                case Test:
                    this.TestemonyOption(context);
                    break;
            }
        }
        private const string FaceEmotionOption = "Face Emotion";
        private const string TextEmotionOption = "Text Emotion";
        private const string TranslateOption = "Translation";

        private void TestemonyOption(IDialogContext context)
        {
            PromptDialog.Choice(context, this.TestemonyOptions, new List<string>() { FaceEmotionOption, TextEmotionOption, TranslateOption }, "Select any one", "Not a valid option", 3);
        }
        private async Task TestemonyOptions(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {

                case FaceEmotionOption:
                    //Calls the DetailsDialog and treats it as a function
                    context.Call(new FaceTestemony(), this.MessageReceivedAsync);
                    break;

                case TextEmotionOption:
                    //Calls the FrequentlyAskedQuestion and treats it as a function
                    context.Call(new TextEmotion(), this.MessageReceivedAsync);
                    break;

                case TranslateOption:
                    //Calls the FrequentlyAskedQuestion and treats it as a function
                    context.Call(new TranslateText(), this.MessageReceivedAsync);
                    break;

            }
        }
        private const string English = "English";
        private const string Marathi = "Marathi";

        private void CounsellerOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnLanguageOptionSelected1, new List<string>() { English, Marathi }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnLanguageOptionSelected1(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case English:
                    //Calls the DetailsDialog and treats it as a function
                    context.Call(new WatsonCounsellorDialog(), this.MessageReceivedAsync);
                    break;

                case Marathi:
                    //Calls the DetailsDialog and treats it as a function
                    context.Call(new WatsonCounsellorMarathiDialog(), this.MessageReceivedAsync);
                    break;
            }
        }

        private static IList<Attachment> GetImagecardsAttachments()
        {
            return new List<Attachment>()
            {
                  GetImagecard(
                    "Welcome To Admission BOT!!!",
                     new CardImage(url: "https://s3-ap-southeast-1.amazonaws.com/mvpar/rockstar.jpg"))

            };
        }

        private static Attachment GetImagecard(string title, CardImage cardImage)
        {
            var imagecard = new HeroCard
            {
                Title = title,
                Images = new List<CardImage>() { cardImage },
            };
            return imagecard.ToAttachment();
        }
    }
}
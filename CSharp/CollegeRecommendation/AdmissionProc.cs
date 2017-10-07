using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System.Net;
using Newtonsoft.Json;
using System.Collections;

namespace CollegeRecommendation
{
    [Serializable]
    public class AdmissionProc : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();
            var attachment = admissionimagecard();
            message.Attachments.Add(attachment);
            await context.PostAsync(message);

            await context.PostAsync("Have You Collected Admission kit from nearest ARC centre?");

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            reply.Attachments = GetStepOneHeroCardAttachments();

            await context.PostAsync(reply);

            context.Wait(StepTwoOptionSelected);
        }

        // Step 2 : 

        private static Attachment admissionimagecard()
        {
            var admission = new HeroCard
            {
                Text = "We'll help you go through all the admission process described by MU",

                Images = new List<CardImage> { new CardImage(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\admissions.jpg") },

            };

            return admission.ToAttachment();
        }

        private async Task StepTwoOptionSelected(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Have you login into DTE using id and password given in the  kit?");

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            reply.Attachments = GetStepTwoHeroCardAttachments();

            await context.PostAsync(reply);

            context.Wait(StepThreeOptionSelected);

        }
        // Step 3: 
        private async Task StepThreeOptionSelected(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Have you fill your details in form !!!");

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            reply.Attachments = GetStepThreeHeroCardAttachments();

            await context.PostAsync(reply);
            // hello python

            context.Wait(StepFourOptionSelected);
        }
        // Step 4 
        private async Task StepFourOptionSelected(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Have you gone to ARC centre to verify documents that you have specified in form !!");

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            reply.Attachments = GetStepFourHeroCardAttachments();

            await context.PostAsync(reply);

            context.Wait(StepFiveOptionSelected);
        }
        // Step 5
        private async Task StepFiveOptionSelected(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Have You Collected Admission kit from nearest ARC centre?");

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            reply.Attachments = GetStepFiveHeroCardAttachments();

            await context.PostAsync(reply);

            context.Wait(StepSixOptionSelected);
        }

        // Step 6 
        private async Task StepSixOptionSelected(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Have you checked the final merit list !");

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            reply.Attachments = GetStepSixHeroCardAttachments();

            await context.PostAsync(reply);

            context.Wait(StepSevenOptionSelected);

        }

        //Step 7

        private async Task StepSevenOptionSelected(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("have you filled the list of colleges in your DTE login!!");

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            reply.Attachments = GetStepSevenHeroCardAttachments();

            await context.PostAsync(reply);

            this.StepEightOptions(context);
        }

        private void StepEightOptions(IDialogContext context)
        {

            var reply = context.MakeMessage();

            reply.Text = "You have completed the Addmission Procedure.... Congrants Now Wait For first result of cap round.";

            context.PostAsync(reply);
            context.Done<object>(null);
        }

        private Attachment GetStepOneHeroCardAttachment()
        {
            var heroCard = new HeroCard
            {
                Title = "ARC Center",
                Subtitle = "Details of arc Center.",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "More Details", value: "https://goo.gl/HxdKr3"),
                    new CardAction(ActionTypes.ImBack, "Step Two", value: "Step Two")

                }
            };

            return heroCard.ToAttachment();
        }

        private static IList<Attachment> GetStepOneHeroCardAttachments()
        {

            List<CardAction> cardButtons = new List<CardAction>();
            //CardAction urlButton = new CardAction()
            //{
            //    Value = "https://goo.gl/SDw2EI",
            //    Type = "OpenUrl",
            //    Title = "Click Here"
            //};
            cardButtons.Add(new CardAction(ActionTypes.OpenUrl, "More Details", value: "https://goo.gl/HxdKr3"));

            //CardAction backButton = new CardAction()
            //{
            //    Value = "back",
            //    Type = "ImBack",
            //    Title = "Completed"

            //};
            cardButtons.Add(new CardAction(ActionTypes.ImBack, "Step Two", value: "Step Two"));

            return new List<Attachment>()
            {
                  GetStepOneHeroCard(

                    "Arc Center",
                     cardButtons)
                   // new CardAction(ActionTypes.OpenUrl, "Click Here", value: "https://goo.gl/SDw2EI"))

            };
        }

        private static IList<Attachment> GetStepTwoHeroCardAttachments()
        {
            List<CardAction> cardButtons = new List<CardAction>();

            cardButtons.Add(new CardAction(ActionTypes.OpenUrl, "More Details", value: "https://goo.gl/Vf6bux"));

            cardButtons.Add(new CardAction(ActionTypes.ImBack, "Step Three", value: "Step Three"));

            return new List<Attachment>()
            {
                  GetStepTwoHeroCard(
                    new CardImage(url: "https://s3-ap-southeast-1.amazonaws.com/chatbotimages/admission/Step2.jpg"),
                    new CardImage(url: "https://s3-ap-southeast-1.amazonaws.com/chatbotimages/admission/Step2_2.jpg"),
                    "Login in arc Center",
                    cardButtons)

            };
        }

        private static IList<Attachment> GetStepThreeHeroCardAttachments()
        {
            List<CardAction> cardButtons = new List<CardAction>();

            cardButtons.Add(new CardAction(ActionTypes.OpenUrl, "More Details", value: "https://goo.gl/Vf6bux"));

            cardButtons.Add(new CardAction(ActionTypes.ImBack, "Step Four", value: "Step Four"));

            return new List<Attachment>()
            {
                  GetStepThreeHeroCard(
                    new CardImage(url: "https://s3-ap-southeast-1.amazonaws.com/chatbotimages/admission/Step3.png"),
                    "To Visit the Website ..",
                   cardButtons)

            };
        }

        private static IList<Attachment> GetStepFourHeroCardAttachments()
        {
            List<CardAction> cardButtons = new List<CardAction>();

            //cardButtons.Add(new CardAction(ActionTypes.OpenUrl, "Click Here", value: "https://goo.gl/Vf6bux"));
            cardButtons.Add(new CardAction(ActionTypes.OpenUrl, "More Details", value: "https://goo.gl/Vf6bux"));
            cardButtons.Add(new CardAction(ActionTypes.ImBack, "Step Five", value: "Step Five"));

            return new List<Attachment>()
            {
                  GetStepFourHeroCard(

                    "Go to same arc center from which you have collected the Admission kit",
                    cardButtons
                  )

            };
        }

        private static IList<Attachment> GetStepFiveHeroCardAttachments()
        {
            List<CardAction> cardButtons = new List<CardAction>();

            cardButtons.Add(new CardAction(ActionTypes.OpenUrl, "More Details", value: "https://goo.gl/LDGNZC"));

            cardButtons.Add(new CardAction(ActionTypes.ImBack, "Step Six", value: "Step Six"));

            return new List<Attachment>()
            {
                  GetStepFiveHeroCard(

                   "To Get address of nearest ARC Center ..",
                  cardButtons

                  )

            };
        }

        private static IList<Attachment> GetStepSixHeroCardAttachments()
        {
            List<CardAction> cardButtons = new List<CardAction>();

            //cardButtons.Add(new CardAction(ActionTypes.OpenUrl, "Click Here", value: "https://goo.gl/LDGNZC"));
            cardButtons.Add(new CardAction(ActionTypes.OpenUrl, "More Details", value: "https://goo.gl/LDGNZC"));

            cardButtons.Add(new CardAction(ActionTypes.ImBack, "Step Seven", value: "Step Seven"));

            return new List<Attachment>()
            {
                  GetStepSixHeroCard(
                    new CardImage(url: "https://s3-ap-southeast-1.amazonaws.com/chatbotimages/admission/Step6.png"),
                    "Please Login to your DTE account ...",
                    cardButtons
                   )

            };
        }

        private static IList<Attachment> GetStepSevenHeroCardAttachments()
        {
            List<CardAction> cardButtons = new List<CardAction>();

            cardButtons.Add(new CardAction(ActionTypes.OpenUrl, "More Details", value: "https://goo.gl/XePIJR"));

            cardButtons.Add(new CardAction(ActionTypes.ImBack, "Step Eight", value: "Step Eight"));


            return new List<Attachment>()
            {
                  GetStepSevenHeroCard(
                   "To Get your status ...",
                  cardButtons
                   )

            };
        }





        private static Attachment GetImageCard(string title, CardImage cardImage)
        {
            var imageCard = new HeroCard
            {
                Title = title,
                Images = new List<CardImage>() { cardImage },

            };

            return imageCard.ToAttachment();
        }

        private static Attachment GetStepOneHeroCard(string title, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = "Admission Reporting Centre (ARC).",
                Text = "Post Provisional Allotment (via CAP process), a candidate needs to report to any of the below Admission Reporting Centres (ARC) and accept the allocated seat by DTE and select Freeze/Slide/Float option.A Candidate who has been allotted a seat shall download the “Provisional Seat Allotment Letter” and pay the remmittance fees as per the DTE. Seat will be confirmed  by  the Admission Reporting Centre (ARC) after verification of the original documents and ensuring that the Candidate meets all the eligibility norms. The centre  in­charge shall issue the Online Receipt of acceptance to the candidate. ",
                Images = new List<CardImage> { new CardImage(AppDomain.CurrentDomain.BaseDirectory + "\\images\\application_form.jpg") },
                Buttons = cardAction
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetStepTwoHeroCard(CardImage cardImage1, CardImage cardImage2, string title, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = "Logging in using ID and Password given in kit.",
                Text = "All  the  MS  candidates  who  have  filled  the  Online  Application  Form  should  report  to any convenient  ARC in person along  with  printout of online  filled  application  form, attested copies  of  the  required documents. The candidate should also carry the required original documents for verification.Post Provisional Allotment (via CAP process), a candidate needs to report to any of the below Admission Reporting Centres (ARC) and accept the allocated seat by DTE and select Freeze/Slide/Float option.A Candidate who has been allotted a seat shall download the “Provisional Seat Allotment Letter” and pay the remmittance fees as per the DTE. Seat will be confirmed  by  the Admission Reporting Centre (ARC) after verification of the original documents and ensuring that the Candidate meets all the eligibility norms. The centre  in­charge shall issue the Online Receipt of acceptance to the candidate. ",
                Images = new List<CardImage> { new CardImage(AppDomain.CurrentDomain.BaseDirectory + "\\images\\login.gif") },
                Buttons = cardAction
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetStepThreeHeroCard(CardImage cardImage, string title, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = "Fill the Details Carefully",
                Text = "The candidate can fill minimum 1 and maximum 300 options. The candidate has to fill the institute choice code against the option number in the online option form.Candidate has to confirm the submitted on - line Option Form himself / herself by re - entering Application ID and Password.The candidate can take the printout of the confirmed Option form for future reference.",
                Images = new List<CardImage> { new CardImage(AppDomain.CurrentDomain.BaseDirectory + "\\images\\details.jpg") },
                Buttons = cardAction
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetStepFourHeroCard(string title, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = "The ARC officer shall verify the information and required original documents and collect the duly signed application along with attested copies of the required documents.",
                Text = "The  ARC  officer  shall  confirm  candidate’s  application  through  online  system  and  issue him / her  the Acknowledge - cum - Receipt  letter,which  will  have  the  particulars of the candidate’s profile,important instructions etc.",
                Images = new List<CardImage> { new CardImage(AppDomain.CurrentDomain.BaseDirectory + "\\images\\verification.jpg") },
                Buttons = cardAction

            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetStepFiveHeroCard(string title, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = "Final merit lists will be displayed on the www.dtemaharashtra.gov.in/fe2014 and at ARCs as per the Schedule.",
                Text = "Provisional Merit List of eligible Maharashtra candidates, TFWS Candidates, All India candidates and J&K Migrant candidates will be displayed on www.dtemaharashtra.gov.in and at the ARCs as per the schedule.",
                Images = new List<CardImage> { new CardImage(AppDomain.CurrentDomain.BaseDirectory + "\\images\\merit.jpg") },
                Buttons = cardAction
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetStepSixHeroCard(CardImage cardImage, string title, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = "It is mandatory for all candidates to confirm the online option form by him / her",
                Text = "Candidates will be able to fill in the online option form through their login on website.It is mandatory for all candidates to confirm the online option form by him / her.The candidate will not be able to change the Options once it is confirmed.",
                Images = new List<CardImage> { new CardImage(AppDomain.CurrentDomain.BaseDirectory + "\\images\\college.jpg") },
                Buttons = cardAction

            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetStepSevenHeroCard(string title, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = "DTE shall display Provisional Allotment of CAP Round I indicating allotted institute and Course.",
                Text = " If candidate fails to report for the acceptance of allotted seat at Admission Reporting Center in scheduled time. It will be treated as if candidate has rejected the allotted seat. However such candidates will be able to participate in subsequent CAP rounds by filling new Option Form.",
                Images = new List<CardImage> { new CardImage(AppDomain.CurrentDomain.BaseDirectory + "\\images\\verification.jpg") },
                Buttons = cardAction
            };

            return heroCard.ToAttachment();
        }
    }
}
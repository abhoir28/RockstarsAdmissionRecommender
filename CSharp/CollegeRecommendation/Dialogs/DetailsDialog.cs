﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System.Net;
using Newtonsoft.Json;

using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using CallingApi;

namespace CollegeRecommendation.Dialogs
{
    [Serializable]
    public class DetailsDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.PostAsync($"Enter Your Name.");

            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync($"Please Select Your Gender.");
            this.GenderOptions(context);


            //context.Call(new Details(), this.ResumeAfter);
        }
        string Category;
        private const string Genral = "Genral Category";
        private const string Minoraty = "Minority";
        private const string Reserved = "Reserved";
        private void CategoryOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnCategoryOptionSelected, new List<string>() { Genral, Minoraty, Reserved }, "Select any one", "Not a valid option", 3);
        }

        private async Task OnCategoryOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case Genral:
                    Category = "Gen";
                    sub = "OPEN";
                    await context.PostAsync($"Select the year.");
                    this.YearOptionSelected(context);
                    break;

                case Minoraty:
                    Category = "MI";
                    this.CategoryMinorityOptionSelected(context);
                    break;

                case Reserved:
                    Category = "Res";
                    this.CategorytwoOptionSelected(context);
                    break;
            }

        }
        string sub;
        private const string Nt1 = "NT1";
        private const string Nt2 = "NT2";
        private const string Nt3 = "NT3";
        private const string St = "ST";
        private const string Obc = "OBC";
        private const string Sc = "SC";
        private const string Vj = "VJ";
        private void CategorytwoOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnMainOptiontwoSelected, new List<string>() { Nt1, Nt2, Nt3, St, Obc, Sc, Vj }, "Select any one", "Not a valid option", 3);
        }

        private async Task OnMainOptiontwoSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case Nt1:
                    sub = "NT1";
                    break;

                case Nt2:
                    sub = "NT2";
                    break;

                case Nt3:
                    sub = "NT3";
                    break;

                case St:
                    sub = "ST";
                    break;

                case Obc:
                    sub = "OBC";
                    break;

                case Sc:
                    sub = "SC";
                    break;

                case Vj:
                    sub = "VJ";
                    break;

            }
            await context.PostAsync($"Select the year.");
            this.YearOptionSelected(context);
        }
        private const string CM = "Christain";
        private const string MM = "Muslim";
        private const string HM = "Hindu";
        private const string GM = "Gujarati";
        private const string SM = "Sindhi";
        private void CategoryMinorityOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnMinorityOptionSelected, new List<string>() { CM, MM, HM, GM, SM }, "Select any one", "Not a valid option", 3);
        }

        private async Task OnMinorityOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case CM:
                    sub = "CM";
                    break;

                case MM:
                    sub = "MM";
                    break;

                case HM:
                    sub = "HM";
                    break;

                case GM:
                    sub = "GM";
                    break;

                case SM:
                    sub = "SM";
                    break;

            }
            await context.PostAsync($"Select the year.");
            this.YearOptionSelected(context);
        }

        private const string First = "First Year";
        private const string Second = "Second Year";

        private void YearOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnYearOptionSelected, new List<string>() { First, Second }, "Select any one", "Not a valid option", 3);
        }

        private async Task OnYearOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case First:
                    break;

                case Second:
                    break;
            }
            await context.PostAsync($"Select the Department.");
            this.MainOptions(context);
        }

        private const string Male = "Male";
        private const string Female = "Female";
        private void GenderOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnGenderOptionSelected, new List<string>() { Male, Female }, "Select any one", "Not a valid option", 3);
        }

        private async Task OnGenderOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case Male:
                    break;

                case Female:
                    break;
            }
            await context.PostAsync($"Please Select Your Category.");
            this.CategoryOptionSelected(context);
        }
        string Stream;
        private const string IT = "Information Technology";
        private const string Coms = "Computer Engineering";
        private const string Civil = "Civil Engineering";
        private const string Mech = "MechanicalEngineering";
        private const string Extc = "ElectronicsandTelecommunicationEngineering";
        private const string Prod = "ProductionEngineering";
        private const string BioTec = "BioTechnology";
        private const string BioMed = "BioMedicalEngineering";
        private const string Chemical = "ChemicalEngineering";
        private const string EEE = "ElectricalEngineering";
        private const string IE = "InstrumentationEngineering";
        private const string Auto = "Automobile";
        private const string ECE = "ElectronicsEngineering";
        private void MainOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnMainOptionSelected, new List<string>() { IT, Coms, Civil, Mech, Extc, BioTec, BioMed, Chemical, IE, EEE, Auto, ECE }, "Select any one", "Not a valid option", 3);
        }

        private async Task OnMainOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case IT:
                    Stream = "InformationTechnology";
                    break;

                case Coms:
                    Stream = "ComputerEngineering";
                    break;

                case Civil:
                    Stream = "CivilEngineering";
                    break;

                case Mech:
                    Stream = "MechanicalEngineering";
                    break;

                case Extc:
                    Stream = "ElectronicsandTelecommunicationEngineering";
                    break;

                case Prod:
                    Stream = "ProductionEngineering";
                    break;

                case BioTec:
                    Stream = "BioTechnology";
                    break;

                case BioMed:
                    Stream = "BioMedicalEngineering";
                    break;

                case Chemical:
                    Stream = "ChemicalEngineering";
                    break;

                case IE:
                    Stream = "InstrumentationEngineering";
                    break;

                case EEE:
                    Stream = "ElectricalEngineering";
                    break;

                case Auto:
                    Stream = "Automobile";
                    break;

                case ECE:
                    Stream = "ElectronicsEngineering";
                    break;
            }

            await context.PostAsync("Enter MHCET Marks");
            context.Wait(CallingApiFunction);
            //context.Done<object>(null);
        }
        string marks;

        private async Task CallingApiFunction(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            marks = activity.Text;

            List<college> listColleges = apiCall(sub, Stream, marks, Category);

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetApiCardsListAttachments(listColleges);
            await context.PostAsync(reply);
            
            context.Call(new FusionQuery(), this.ResumeAferEmotion);

            //await context.PostAsync("Did you get your Desired College");
           // context.Wait(DesiredCollege);
        }

        private async Task ResumeAferEmotion(IDialogContext context, IAwaitable<object> result)
        {
            FusionQuery fq = new FusionQuery();
            
            context.Wait(fq.TextEmo);
            await context.PostAsync("It seems Like");

        }

        private static List<college> apiCall(string subCategory, string department, string marks,string category)
        {
            string myCategory = category;
            string mySubCategory = subCategory;
            string myDepartment = department;

            //var subCategoryValue = subCategory;


            //if (mySubCategory == "Christain")
            //    subCategoryValue = "CM";

            //else if (mySubCategory == "Muslim")
            //    subCategoryValue = "MM";

            //else if (mySubCategory == "Hindu")
            //    subCategoryValue = "HM";

            //else if (mySubCategory == "Gujarati")
            //    subCategoryValue = "GM";

            //else if (mySubCategory == "Sindhi")
            //    subCategoryValue = "SM";


            //Dictionary<string, string> departmentValues = new Dictionary<string, string>();
            //departmentValues.Add("EXTC", "ElectronicsandTelecommunicationEngineering");
            //departmentValues.Add("IT", "InformationTechnology");
            //departmentValues.Add("COMPS", "ComputerEngineering");
            //departmentValues.Add("CIVIL", "CivilEngineering");
            //departmentValues.Add("MECH", "MechanicalEngineering");
            //departmentValues.Add("BioTech", "BioTechnology");
            //departmentValues.Add("BioMedicalEngineering", "BioMedicalEngineering");
            //departmentValues.Add("CHEMICAL", "ChemicalEngineering");
            //departmentValues.Add("EEE", "ElectricalEngineering");
            //departmentValues.Add("IE", "InstrumentationEngineering");
            //departmentValues.Add("PRODUCTION", "ProductionEngineering");
            //departmentValues.Add("AUTOMOBILE", "Automobile");
            //departmentValues.Add("ECE", "ElectronicsEngineering");

            string departmentSelected = department;

            // departmentValues.TryGetValue(mySubCategory, out departmentSelected);


            WebClient webClient = new WebClient();
            /*webClient.QueryString.Add("marks", marks);
            // webClient.QueryString.Add("category", myCategory);
            //  webClient.QueryString.Add("department", myDepartment);
            webClient.QueryString.Add("category", myCategory);
            webClient.QueryString.Add("subCategory", subCategoryValue);
            webClient.QueryString.Add("department", departmentSelected);*/


            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("marks", marks);
            values.Add("category", myCategory);
            values.Add("subCategory", mySubCategory);
            values.Add("department", departmentSelected);

            Dictionary<string, string> myvalues = new Dictionary<string, string>();
            myvalues.Add("minCet", marks);
            myvalues.Add("category", myCategory);
            myvalues.Add("subCategory", mySubCategory);
            myvalues.Add("department", departmentSelected);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            string JsonString = (new JavaScriptSerializer()).Serialize(values);


            DataContractJsonSerializer myserializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            string myJsonString = (new JavaScriptSerializer()).Serialize(myvalues);

            //string serializedObject = JsonConvert.SerializeObject(myQueryStringCollection);
            //string result = webClient.DownloadString("https://g0zmgbj2z2.execute-api.ap-southeast-1.amazonaws.com/prod12");
            string result = webClient.UploadString("https://g0zmgbj2z2.execute-api.ap-southeast-1.amazonaws.com/prod12", "POST", JsonString);
            dynamic parsedArray = JsonConvert.DeserializeObject(result);

            string result2 = webClient.UploadString("https://jw5zc9iyjh.execute-api.ap-southeast-1.amazonaws.com/dev/", "POST", myJsonString);


            dynamic parsedArrayTwo = JsonConvert.DeserializeObject(result2);





            dynamic parsedArray2 = JsonConvert.DeserializeObject(parsedArray);

            college cg;
            List<college> clg = new List<college>();

            foreach (dynamic item in parsedArrayTwo)
            {
                cg = new college();
                cg.collegeName = item.collegeName;
                cg.Description = item.description;
                cg.imgUrl = item.imgUrl;
                cg.address = item.address;
                cg.webName = item.webName;
                clg.Add(cg);

            }

            return clg;

        }
        private IList<Attachment> GetApiCardsListAttachments(List<college> listColleges)
        {
            List<Attachment> attachment = new List<Attachment>();

            foreach (college c in listColleges)
            {

                var resAttach = GetHeroListCard(
                    c.collegeName,
                    c.address,
                    c.Description,
                    new CardImage(url: c.imgUrl),
                    new CardAction(ActionTypes.OpenUrl, "Learn More", value: c.webName));

                attachment.Add(resAttach);
            }
            return attachment;
        }

        private static Attachment GetHeroListCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
        private async Task DesiredCollege(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var res = activity.Text;
            int Mrk = Convert.ToInt32(marks);
            if (res.Contains("no") || res.Contains("none") || res.Contains("No") || res.Contains("None"))
            {
                if (Mrk > 150)
                {
                    this.MhcetLessThanOnetwentySelected(context);
                }
                else if (Mrk > 120)
                {
                    this.MhcetLessThanEightySelected(context);
                }
                else
                {
                    this.MhcetLessThanFourtySelected(context);
                }
            }
            else
            {
                context.Done<object>(null);
            }
        }

        private const string Watumall = "Watumall";
        private const string Rizvi = "Rizvi";
        private const string Vidyavardani = "Vidyavardhini's College of Engineering and Technology";
        private const string Viva = "VIVA Institute of Technology";
        private const string Universal = "Universal College of Engineering Mumbai";
        private void MhcetLessThanFourtySelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnMhcetLessThanFourtySelected, new List<string>() { Watumall, Rizvi, Vidyavardani, Viva, Universal }, "Select any one", "Not a valid option", 3);
        }
        string CollegeName;
        private async Task OnMhcetLessThanFourtySelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case Watumall:
                    CollegeName = "Watumull Institute of Electronics Engineering and Computer Technology";
                    break;

                case Rizvi:
                    CollegeName = "Rizvi Education Society's Rizvi College of Engineering (RCOE)";
                    break;

                case Vidyavardani:
                    CollegeName = "Vidyavardhini's College of Engineering and Technology";
                    break;

                case Viva:
                    CollegeName = "VIVA Institute of Technology";
                    break;

                case Universal:
                    CollegeName = "Universal College of Engineering Mumbai";
                    break;

            }

            await context.PostAsync("These are the colleges in which you can get in Irrespective of Stream.");
            List<college> listColleges = SecondApiCall(sub, marks, Category, CollegeName);

            var reply = context.MakeMessage();

            //reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetApiCardsCollegeAttachments(listColleges);

            // await context.PostAsync(msg);
            await context.PostAsync(reply);
            context.Done<object>(null);
        }
        private const string Xaviers = "Xaviers";
        private const string Thadomal = "Thadomal";
        private const string Tiwari = "Tiwari";
        private const string John = "John";
        private const string Theems = "Theem College of Engineering";
        private const string Rjit = "Manjara Charitable Trust's Rajiv Gandhi Institude of Technology (RGIT)";
        private const string Bhavans = "Bhartiya Vidya Bhavans, Sardar Patel College of Engineering (SPCE)";
        private const string Atharva = "Atharva College of Engineering ( ACE )";
        private const string Thakur = "Thakur College of Engineering and Technology (TCET)";
        private const string Francis = "St. Francis Institute of Technology ( Popularly known as SFIT )";
        private void MhcetLessThanEightySelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnMhcetLessThanEightySelected, new List<string>() { Watumall, Rizvi, Vidyavardani, Viva, Universal, Xaviers, Thadomal, Tiwari, John, Theems, Rjit, Bhavans, Atharva, Thakur, Francis }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnMhcetLessThanEightySelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case Watumall:
                    CollegeName = "Watumull Institute of Electronics Engineering and Computer Technology";
                    break;

                case Rizvi:
                    CollegeName = "Rizvi Education Society's Rizvi College of Engineering (RCOE)";
                    break;

                case Vidyavardani:
                    CollegeName = "Vidyavardhini's College of Engineering and Technology";
                    break;

                case Viva:
                    CollegeName = "VIVA Institute of Technology";
                    break;

                case Universal:
                    CollegeName = "Universal College of Engineering Mumbai";
                    break;

                case Xaviers:
                    CollegeName = "Xavier Institute of Engineering (XIE)";
                    break;

                case Thadomal:
                    CollegeName = "Thadomal Shahani Engineering College (TSEC)";
                    break;

                case Tiwari:
                    CollegeName = "Shree L R Tiwari College of Engineering";
                    break;

                case John:
                    CollegeName = "St. John College of Engineering and Technology";
                    break;

                case Theems:
                    CollegeName = "Theem College of Engineering";
                    break;

                case Rjit:
                    CollegeName = "Manjara Charitable Trust's Rajiv Gandhi Institude of Technology (RGIT)";
                    break;

                case Bhavans:
                    CollegeName = "Bhartiya Vidya Bhavans, Sardar Patel College of Engineering (SPCE)";
                    break;

                case Atharva:
                    CollegeName = "Atharva College of Engineering ( ACE )";
                    break;

                case Thakur:
                    CollegeName = "Thakur College of Engineering and Technology (TCET)";
                    break;

                case Francis:
                    CollegeName = "St. Francis Institute of Technology ( Popularly known as SFIT )";
                    break;
            }

            await context.PostAsync("These are the colleges in which you can get in Irrespective of Stream.");
            List<college> listColleges = SecondApiCall(sub, marks, Category, CollegeName);

            var reply = context.MakeMessage();

            //reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetApiCardsCollegeAttachments(listColleges);

            // await context.PostAsync(msg);
            await context.PostAsync(reply);
            context.Done<object>(null);
        }
        private const string Somaiya = "K.J.Somaiya College of Engineering";
        private const string Sangvi = "Shri Vile Parle Kelavani Mandal's Dwarkadas J. Sanghvi College of Engineering (Popularly known as DJ Sanghvi)";
        private const string Concei = "Fr. Conceicao Rodrigues College Of Engineering";

        private void MhcetLessThanOnetwentySelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnMhcetLessThanOnetwentySelected, new List<string>() { Watumall, Rizvi, Vidyavardani, Viva, Universal, Xaviers, Thadomal, Tiwari, John, Theems, Rjit, Bhavans, Atharva, Thakur, Francis }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnMhcetLessThanOnetwentySelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case Watumall:
                    CollegeName = "Watumull Institute of Electronics Engineering and Computer Technology";
                    break;

                case Rizvi:
                    CollegeName = "Rizvi Education Society's Rizvi College of Engineering (RCOE)";
                    break;

                case Vidyavardani:
                    CollegeName = "Vidyavardhini's College of Engineering and Technology";
                    break;

                case Viva:
                    CollegeName = "VIVA Institute of Technology";
                    break;

                case Universal:
                    CollegeName = "Universal College of Engineering Mumbai";
                    break;

                case Xaviers:
                    CollegeName = "Xavier Institute of Engineering (XIE)";
                    break;

                case Thadomal:
                    CollegeName = "Thadomal Shahani Engineering College (TSEC)";
                    break;

                case Tiwari:
                    CollegeName = "Shree L R Tiwari College of Engineering";
                    break;

                case John:
                    CollegeName = "St. John College of Engineering and Technology";
                    break;

                case Theems:
                    CollegeName = "Theem College of Engineering";
                    break;

                case Rjit:
                    CollegeName = "Manjara Charitable Trust's Rajiv Gandhi Institude of Technology (RGIT)";
                    break;

                case Bhavans:
                    CollegeName = "Bhartiya Vidya Bhavans, Sardar Patel College of Engineering (SPCE)";
                    break;

                case Atharva:
                    CollegeName = "Atharva College of Engineering ( ACE )";
                    break;

                case Thakur:
                    CollegeName = "Thakur College of Engineering and Technology (TCET)";
                    break;

                case Francis:
                    CollegeName = "St. Francis Institute of Technology ( Popularly known as SFIT )";
                    break;

                case Somaiya:
                    CollegeName = "K.J.Somaiya College of Engineering";
                    break;

                case Sangvi:
                    CollegeName = "Shri Vile Parle Kelavani Mandal's Dwarkadas J. Sanghvi College of Engineering (Popularly known as DJ Sanghvi)";
                    break;

                case Concei:
                    CollegeName = "Fr. Conceicao Rodrigues College Of Engineering";
                    break;
            }

            await context.PostAsync("These are the colleges in which you can get in Irrespective of Stream.");
            List<college> listColleges = SecondApiCall(sub, marks, Category, CollegeName);

            var reply = context.MakeMessage();

            //reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetApiCardsCollegeAttachments(listColleges);

            await context.PostAsync(reply);
            context.Done<object>(null);
        }

        private static List<college> SecondApiCall(string subCategory, string marks, string category, string college)
        {
            string myCategory = category;
            string mySubCategory = subCategory;


            WebClient webClient = new WebClient();


            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("minCet", marks);
            values.Add("category", myCategory);
            values.Add("subCategory", mySubCategory);
            values.Add("collegeName", college);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            string JsonString = (new JavaScriptSerializer()).Serialize(values);


            //string serializedObject = JsonConvert.SerializeObject(myQueryStringCollection);
            //string result = webClient.DownloadString("https://g0zmgbj2z2.execute-api.ap-southeast-1.amazonaws.com/prod12");
            //string result = webClient.UploadString("https://g0zmgbj2z2.execute-api.ap-southeast-1.amazonaws.com/prod12", "POST", JsonString);
            //dynamic parsedArray = JsonConvert.DeserializeObject(result);

            string result2 = webClient.UploadString("https://goocg51m9i.execute-api.ap-southeast-1.amazonaws.com/dev/", "POST", JsonString);


            dynamic parsedArrayTwo = JsonConvert.DeserializeObject(result2);

            //dynamic parsedArray2 = JsonConvert.DeserializeObject(parsedArray);

            college cg;
            List<college> clg = new List<college>();
            int i = 0;

            foreach (dynamic item in parsedArrayTwo)
            {
                cg = new college();
                cg.collegeName = item.collegeName;
                cg.Description = item.description;
                cg.imgUrl = item.imgUrl;
                cg.department = item.department;
                cg.fees = item.fees;
                cg.address = item.address;
                cg.webName = item.webName;
                clg.Add(cg);

            }

            return clg;

        }
        private IList<Attachment> GetApiCardsCollegeAttachments(List<college> listColleges)
        {
            List<Attachment> attachment = new List<Attachment>();
            int i = 0;
            foreach (college c in listColleges)
            {
                if (i == 0)
                {
                    var resAttach = GetHeroCollegeCard(
                    c.collegeName,
                    c.address,
                    c.Description,
                    new CardImage(url: c.imgUrl),
                    new CardAction(ActionTypes.OpenUrl, c.department, value: c.webName));
                    attachment.Add(resAttach);
                    i = i + 1;

                }
                else
                {
                    var resAttach = GetHeroColCard(

                    new CardAction(ActionTypes.OpenUrl, c.department, value: c.webName));
                    attachment.Add(resAttach);
                }
            }
            return attachment;
        }

        private static Attachment GetHeroCollegeCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
        private static Attachment GetHeroColCard(CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }

    }

}
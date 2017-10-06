using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;

namespace KJHackApplication4.Dialogs
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
            await context.PostAsync($"Please Select Your Category.");
            this.CategoryOptionSelected(context);


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
            await context.PostAsync($"Select the Gender");
            this.GenderOptions(context);
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
            await context.PostAsync($"Select the Department");
            this.MainOptions(context);
        }
        string Stream;
        private const string IT = "Information Technology";
        private const string Coms = "Computer Engineering";
        private const string Civil = "Civil Engineering";
        private const string Mech = "Mechanical Engineering";
        private const string Extc = "Electronics and Telecommunication Engineering";
        private const string Prod = "Production Engineering";
        private const string BioTec = "Bio Technology";
        private const string BioMed = "Bio Medical Engineering";
        private const string Chemical = "Chemical Engineering";
        private const string EEE = "Electrical Engineering";
        private const string IE = "Instrumentation Engineering";
        private const string Auto = "Automobile";
        private const string ECE = "Electronics Engineering";
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
            context.Wait(MhCetMarks);
        }
        string marks;
        private async Task MhCetMarks(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            marks = activity.Text;
        }

    }

}
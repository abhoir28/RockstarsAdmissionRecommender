using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace KJHackApplication4.Dialogs
{
    public class LocationDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            this.LocationOptionSelected(context);

            return Task.CompletedTask;
        }
        private const string Virar = "Boisar to Virar";
        private const string Borivali = "Vasai to Borivali";
        private const string Andheri = "Kandivali to Andheri";
        private const string Bandra = "Bandra to Churchgate";
        private const string Central = "Dadar to Thane";
        private void LocationOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnLocationOptionSelected, new List<string>() { Virar, Borivali, Andheri, Bandra, Central }, "Select any one", "Not a valid option", 3);
        }

        private async Task OnLocationOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            switch (optionSelected)
            {
                case Virar:
                    this.VirarOptionSelected(context);
                    break;

                case Borivali:
                    this.BorivaliOptionSelected(context);
                    break;

                case Andheri:
                    this.AndheriOptionSelected(context);
                    break;

                case Bandra:
                    this.BandraOptionSelected(context);
                    break;

                case Central:
                    this.CentralOptionSelected(context);
                    break;
            }
            await context.PostAsync("Select any one College.");
        }
        private const string Theems = "Theems";
        private const string John = "St. John";
        private const string Viva = "Viva";
        string CollegeName;
        private void VirarOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnVirarOptionSelected, new List<string>() { Theems, John, Viva }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnVirarOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionselected = await result;
            switch(optionselected)
            {
                case Theems:
                    CollegeName = "Theems";
                    break;

                case John:
                    CollegeName = "St.John";
                    break;

                case Viva:
                    CollegeName = "Viva";
                    break;
            }

        }
        private const string Francis = "Francis";
        private const string Universal = "Universal";
        private const string Tiwari = "L R Tiwari";
        private void BorivaliOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnBorivaliOptionSelected, new List<string>() { Francis, Universal, Tiwari }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnBorivaliOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionselected = await result;
            switch (optionselected)
            {
                case Francis:
                    CollegeName = "Francis";
                    break;

                case Universal:
                    CollegeName = "Universal";
                    break;

                case Tiwari:
                    CollegeName = "Tiwari";
                    break;

            }
        }
        private const string Athavra = "Athavra";
        private const string Thakur = "Thakur";
        private const string Rjit = "RJIT";
        private const string Bhavans = "Bhavans";
        private void AndheriOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnAndheriOptionSelected, new List<string>() { Athavra, Thakur, Rjit, Bhavans }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnAndheriOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionselected = await result;
            switch (optionselected)
            {
                case Athavra:
                    CollegeName = "Atharva";
                    break;

                case Thakur:
                    CollegeName = "Thakur";
                    break;

                case Rjit:
                    CollegeName = "Rjit";
                    break;

                case Bhavans:
                    CollegeName = "Bhavans";
                    break;
            }

        }
        private const string Xaviers = "Xaviers";
        private const string Concecio = "Conceicao";
        private const string Sanghvi = "D J Sanghvi";
        private const string Thadomal = "Thadomal";

        private void BandraOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnBandraOptionSelected, new List<string>() { Xaviers, Concecio, Sanghvi, Thadomal }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnBandraOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionselected = await result;
            switch (optionselected)
            {
                case Xaviers:
                    CollegeName = "Atharva";
                    break;

                case Concecio:
                    CollegeName = "Conceicao";
                    break;

                case Sanghvi:
                    CollegeName = "Sanghvi";
                    break;

                case Thadomal:
                    CollegeName = "Thadomal";
                    break;

            }

        }
        private const string Saboo = "Saboo Siddhik";
        private const string Somaiya = "Somaiya";
        private void CentralOptionSelected(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnCentralOptionSelected, new List<string>() { Saboo, Somaiya }, "Select any one", "Not a valid option", 3);
        }
        private async Task OnCentralOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionselected = await result;
            switch (optionselected)
            {
                case Saboo:
                    CollegeName = "Saboo";
                    break;

                case Somaiya:
                    CollegeName = "Somaiya";
                    break;
            }
        }
    }

}
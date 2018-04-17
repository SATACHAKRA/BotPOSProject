using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public int noneattempt = 0;
        public int issueattempt = 0;
        public int posattempt = 0;
        public int eidattempt = 0;
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }
        public List<string> none = new List<string>
        {
            "Sorry, I cannot understand your query.",
            "Sorry, I am unable to handle that.",
            "Sorry, I am not trained for that.",
            "Sorry, I did not get you.",
            "Sorry, I did not follow."
        };
        public List<string> issue = new List<string> { "POS Open Fail", "Merge EID" };
        public string myissue;
        public string theissue;
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            noneattempt++;
            string mynone = new GlobalHandler().GetRandomString(none);
            if (noneattempt >= 2)
            {
                await context.SayAsync(text: mynone+"...", speak: mynone);
                await new TransferToAPerson().StartAsync(context);
            }
            else
            {
                await context.SayAsync(text: mynone + " Try again...", speak: mynone + " Try again.", options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
                context.Wait(this.MessageReceived);
            }
        }
        [LuisIntent("Issue")]
        public async Task IssueIntent(IDialogContext context, LuisResult result)
        {
            string prompt = "Okay, tell me what is your issue?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: issue, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, Issue, promptOptions);
        }
        public async Task Issue(IDialogContext context, IAwaitable<string> result)
        {
            this.myissue = await result;
            if (myissue.ToLower().Contains("pos") || myissue.ToLower().Contains("open") || myissue.ToLower().Contains("fail"))
            {
                await new POSOpenFail().StartAsync(context);
            }
            else if (myissue.ToLower().Contains("eid") || myissue.ToLower().Contains("merge") || myissue.ToLower().Contains("id"))
            {
                await new EIDMerge().StartAsync(context);
            }
        }
        [LuisIntent("ReportOpenFailError")]
        public async Task ReportOpenFailErrorIntent(IDialogContext context, LuisResult result)
        {
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = "Are you calling for POS Open Fail?";
            string retryprompt = "Please try again";
            string promptspeak = "Are you calling for p o s open fail?";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: promptspeak, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmPOS, promptOptions);
        }
        public async Task ConfirmPOS(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower() == "yes")
            {
                await new POSOpenFail().StartAsync(context);
            }
            else
            {
                posattempt++;
                if (posattempt == 2)
                {
                    await new TransferToAPerson().StartAsync(context);
                }
                else
                {
                    await context.SayAsync(text: "Let's try again", speak: "Let's try again");
                    await POS(context);
                }
            }
        }
        public async Task POS(IDialogContext context)
        {
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = "Are you calling for POS Open Fail?";
            string retryprompt = "Please try again";
            string promptspeak = "Are you calling for p o s open fail?";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: promptspeak, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmPOS, promptOptions);
        }
        [LuisIntent("EIDMerge")]
        public async Task EIDMergeIntent(IDialogContext context, LuisResult result)
        {
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = "Are you calling for Merge EID";
            string retryprompt = "Please try again";
            string promptspeak = "Are you calling for merge e i d?";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: promptspeak, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmEID, promptOptions);
        }
        public async Task ConfirmEID(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower() == "yes")
            {
                await new EIDMerge().StartAsync(context);
            }
            else
            {
                eidattempt++;
                if (eidattempt == 2)
                {
                    await new TransferToAPerson().StartAsync(context);
                }
                else
                {
                    await context.SayAsync(text: "Let's try again", speak: "Let's try again");
                    await EID(context);
                }
            }
        }
        public async Task EID(IDialogContext context)
        {
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = "Are you calling for Merge EID";
            string retryprompt = "Please try again";
            string promptspeak = "Are you calling for merge e i d?";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: promptspeak, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmEID, promptOptions);
        }
    }
}
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class POSOpenFail : BasicLuisDialog, IDialog<object>
    {
        public int attempt = 0;
        static GlobalHandler.POS pos = new GlobalHandler.POS();
        string step1 = pos.Step1;
        string step2 = pos.Step2;
        public async Task StartAsync(IDialogContext context)
        {
            string s = "If you do not have a P O S ID and password, please have a store manager log in.";
            await context.SayAsync(text: step1, speak: step1);
            await context.SayAsync(text: step2, speak: s);
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = "Do you confirm?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmResume, promptOptions);
        }
        public async Task ConfirmResume(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower() == "yes")
            {
                await new ConfirmReady().StartAsync(context);
            }
            else
            {
                attempt++;
                if (attempt >= 2)
                {
                    await context.SayAsync(text: "Let me transfer you to a person for your better understanding", speak: "Let me transfer you to a person for your better understanding");
                    await new TransferToAPerson().StartAsync(context);
                }
                else
                {
                    string s = "If you do not have a P O S ID and password, please have a store manager log in.";
                    await context.SayAsync(text: "It seems you have some confusion.", speak: "It seems you have some confusion.");
                    await context.SayAsync(text: "Let me repeat the steps for you", speak: "Let me repeat the steps for you");
                    await context.SayAsync(text: step1, speak: step1);
                    await context.SayAsync(text: step2, speak: s);
                    await Confirm(context);
                }
            }
        }
        public async Task Confirm(IDialogContext context)
        {
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = "Do you confirm this time?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmResume, promptOptions);
        }
    }
}
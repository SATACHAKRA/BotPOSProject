using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class TransferToAPerson : BasicLuisDialog, IDialog<object>
    {
        public List<string> transfer = new List<string> { "Yes", "No" };
        public async Task StartAsync(IDialogContext context)
        {
            string prompt = "Do you want to be transferred now?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: transfer, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, TransferConfirmed, promptOptions);
        }
        public async Task TransferConfirmed(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower().Contains("yes"))
            {
                await context.SayAsync(text: "You are now being transferred to a customer service representative...", speak: "You are now being transferred to a customer service representative");
                context.Wait(this.MessageReceived);
                
            }
            else if (confirm.ToLower().Contains("no"))
            {
                var incidentNumber = string.Empty;
                await new CreateServiceRequest().Start(context, incidentNumber);
            }
        }
    }
}
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class CloseContact : BasicLuisDialog, IDialog<object>
    {
        static GlobalHandler.Close close = new GlobalHandler.Close();
        List<string> HelpMessage = close.HelpMessage;
        List<string> RestartMessage = close.RestartMessage;
        public async Task StartAsync(IDialogContext context)
        {
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = new GlobalHandler().GetRandomString(HelpMessage);
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, Confirm, promptOptions);
        }
        public async Task Confirm(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower() == "no")
            {
                await new EndCall().StartAsync(context);
            }
            else
            {
                string restart = new GlobalHandler().GetRandomString(RestartMessage);
                await context.SayAsync(text: $"{restart} You can try asking me things like 'POS error', 'EID Merge', etc", speak: $"{restart} You can try asking me things like 'P O S Error', 'E I D Merge', etc", options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
                context.Wait(this.MessageReceived);
            }
        }
    }
}
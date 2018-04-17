using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace POSBot
{
    public class CancelResponse : RootDialog, IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync(text: "Hi, are you having a POS or EID issue?", speak: "Hi, are you having a P O S or E I D issue?", options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(MessageReceivedAsync);
        }
    }
}
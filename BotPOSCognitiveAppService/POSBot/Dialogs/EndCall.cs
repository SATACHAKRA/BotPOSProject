using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class EndCall : BasicLuisDialog, IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync(text: "Thank you very much for calling.", speak: "Thank you very much for calling.");
            context.Wait(this.MessageReceived);
        }
    }
}
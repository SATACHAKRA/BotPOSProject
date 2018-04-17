using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace POSBot
{
    public class PersonAssist : IDialog<object>
    {
        static GlobalHandler.EID eid = new GlobalHandler.EID();
        List<string> level = eid.level;
        public async Task StartAsync(IDialogContext context)
        {
            string prompt = "Is the Person Merge needed for a store level employee or staff level employee?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: level, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, new EIDMerge().Level, promptOptions);
        }
    }
}
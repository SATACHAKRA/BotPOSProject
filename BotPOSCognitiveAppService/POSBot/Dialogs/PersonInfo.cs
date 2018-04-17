using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace POSBot
{
    public class PersonInfo : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync(text: "A Person Merge is required whenever an employee who has training associated with a prior EID acquires a new EID.The Person Merge process transfers all learning transcripts to the new EID to ensure the employee's records include all past training. This allows employees to have one training transcript with their entire training history.", speak: "A Person Merge is required whenever an employee who has training associated with a prior E I D acquires a new E I D. The Person Merge process transfers all learning transcripts to the new E I D to ensure the employee's records include all past training. This allows employees to have one training transcript with their entire training history. A store was acquired by a new O/O and the new O/O created new e i ds for managers.");
            await context.SayAsync(text: "An employee may require a Person Merge in a number of situations, including the following: <br/>• The employee has transferred to a new organization and a new EID was created for them <br/>• The employee left the company and came back and new EID was created for them <br/>• EIDs can be disabled manually when an employee leaves the company <br/>• EIDs are disabled automatically after 6 months of inactivity <br/>• The employee was promoted from crew trainer to manager <br/>• A store was acquired by a new O/ O and the new O/ O created new EIDs for managers", speak: "An employee may require a Person Merge in a number of situations, including the following. The employee has transferred to a new organization and a new e i d was created for them. The employee left the company and came back and new e i d was created for them. E i ds can be disabled manually when an employee leaves the company. E i ds are disabled automatically after 6 months of inactivity. The employee was promoted from crew trainer to manager. A store was acquired by a new O/O and the new O/O created new EIDs for managers.");
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = "Do you need assistance with a Person Merge now?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, new EIDMerge().InfoResume, promptOptions);
        }
    }
}
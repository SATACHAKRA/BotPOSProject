using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class EIDMerge : BasicLuisDialog, IDialog<object>
    {
        static GlobalHandler.EID eid = new GlobalHandler.EID();
        List<string> assistance = eid.assistance;
        List<string> level = eid.level;
        List<string> support = eid.support;
        List<string> merge = eid.merge;
        public async Task StartAsync(IDialogContext context)
        {
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = new GlobalHandler().GetRandomString(merge);
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, MergeConfirm, promptOptions);
        }
        public async Task MergeConfirm(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower() == "yes")
            {
                await new CreateLMSTicket().StartAsync(context);
            }
            else
            {
                await context.SayAsync(text: "A Person Merge needs to be completed.", speak: "A Person Merge needs to be completed.");
                string help = "Do you need assistance with the Person Merge Process, information about what Person Merge is, or training and instructions for Global Account Manager?";
                await context.SayAsync(text: help, speak: help);
                string prompt = "Please select one option";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: assistance, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, Assistance, promptOptions);
            }
        }
        public async Task Assistance(IDialogContext context, IAwaitable<string> result)
        {
            string assistance = await result;
            if (assistance.ToLower().Contains("person merge assistance"))
            {
                await PersonAssist(context);
            }
            else if (assistance.ToLower().Contains("person merge information"))
            {
                await PersonInfo(context);
            }
            else if (assistance.ToLower().Contains("global account manager"))
            {
                await GAMTraining(context);
            }
        }
        public async Task PersonAssist(IDialogContext context)
        {
            string prompt = "Is the Person Merge needed for a store level employee or staff level employee?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: level, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, Level, promptOptions);
        }
        public async Task PersonInfo(IDialogContext context)
        {
            await context.SayAsync(text: "A Person Merge is required whenever an employee who has training associated with a prior EID acquires a new EID.The Person Merge process transfers all learning transcripts to the new EID to ensure the employee's records include all past training. This allows employees to have one training transcript with their entire training history.", speak: "A Person Merge is required whenever an employee who has training associated with a prior E I D acquires a new E I D. The Person Merge process transfers all learning transcripts to the new E I D to ensure the employee's records include all past training. This allows employees to have one training transcript with their entire training history. A store was acquired by a new O/O and the new O/O created new e i ds for managers.");
            await context.SayAsync(text: "An employee may require a Person Merge in a number of situations, including the following: <br/>• The employee has transferred to a new organization and a new EID was created for them <br/>• The employee left the company and came back and new EID was created for them <br/>• EIDs can be disabled manually when an employee leaves the company <br/>• EIDs are disabled automatically after 6 months of inactivity <br/>• The employee was promoted from crew trainer to manager <br/>• A store was acquired by a new O/ O and the new O/ O created new EIDs for managers", speak: "An employee may require a Person Merge in a number of situations, including the following. The employee has transferred to a new organization and a new e i d was created for them. The employee left the company and came back and new e i d was created for them. E i ds can be disabled manually when an employee leaves the company. E i ds are disabled automatically after 6 months of inactivity. The employee was promoted from crew trainer to manager. A store was acquired by a new O/O and the new O/O created new EIDs for managers.");
            List<string> choices = new List<string> { "Yes", "No" };
            string prompt = "Do you need assistance with a Person Merge now?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, InfoResume, promptOptions);
        }
        public async Task GAMTraining(IDialogContext context)
        {
            string gam = "For training, instructions, and additional information on Global Account Manager, please see the Global Account Manager Launch and Learn presentation on Access, or contact GAM Support at GlobalAMSupport@us.com";
            await context.SayAsync(text: gam, speak: gam);
            await context.SayAsync(text: "Press this link to see the presentation: https://www.canva.com", speak: "Press this link to see the presentation");
            await context.SayAsync(text: "Press this link to contact us: https://www.canva.com", speak: "Press this link to contact us");
            var incidentNumber = "E" + new Random().Next(1000, 9999);
            await new CreateServiceRequest().Start(context, incidentNumber);
        }
        public async Task InfoResume(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            
            if (confirm.ToLower() == "yes")
            {
                await PersonAssist(context);
            }
            else
            {
                await context.SayAsync(text: "So you do not need assistance.", speak: "So you do not need assistance.");
                var incidentNumber = "E" + new Random().Next(1000, 9999);
                await new CreateServiceRequest().Start(context, incidentNumber);
            }
        }
        public async Task Level(IDialogContext context, IAwaitable<string> result)
        {
            string level = await result;
            if (level.ToLower().Contains("staff"))
            {
                await context.SayAsync("Contact the Corporate help desk through service cafe or at 555-555-5555 to assist with the EID merge.", speak: "Contact the Corporate help desk through service cafe or at 555-555-5555 to assist with the e i d merge.");

                var incidentNumber = "E" + new Random().Next(1000, 9999);
                await new CreateServiceRequest().Start(context, incidentNumber);
            }
            else if (level.ToLower().Contains("store"))
            {
                await new CreateLMSTicket().StartAsync(context);
            }
        }
    }
}
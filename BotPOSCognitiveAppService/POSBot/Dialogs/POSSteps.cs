using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace POSBot
{
    public class POSSteps : IDialog<object>
    {
        public IMessageActivity activity;
        string s = "Step 1: Click on Manager Menu from P O S screen.";
        List<string> steptext = new List<string> { " ", con.Step1, con.Step2, con.Step3, con.Step4 };
        List<int> list = new List<int> { 0 };
        static GlobalHandler.Confirm con = new GlobalHandler.Confirm();
        public POSSteps(IMessageActivity activity)
        {
            this.activity = activity;
        }
        public async Task StartAsync(IDialogContext context)
        {
            int flag = 0;
            foreach (var i in this.activity.Text.ToCharArray())
            {
                if (char.IsNumber(i) && i <= '4')
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                foreach (var i in this.activity.Text.ToCharArray())
                {
                    if (char.IsNumber(i) && i <= '4')
                    {
                        list.Add(i - 48);
                    }
                }
                if (this.activity.Text.Contains("from"))
                {
                    for (int i = list[1]; i < steptext.Count; i++)
                    {
                        if (i == 1)
                        {
                            await context.SayAsync(text: steptext[i], speak: s);
                        }
                        else
                        {
                            await context.SayAsync(text: steptext[i], speak: steptext[i]);
                            //await new GlobalHandler().makessml(context, steptext[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < list.Count; i++)
                    {
                        if (list[i] == 1)
                        {
                            await context.SayAsync(text: steptext[list[i]], speak: s);
                        }
                        else
                        {
                            await context.SayAsync(text: steptext[list[i]], speak: steptext[list[i]]);
                        }
                        //await new GlobalHandler().makessml(context, steptext[list[i]]);
                    }
                }
                await Progress(context);
            }
            else
            {
                if (this.activity.Text.Contains("from"))
                {
                    await ValidStep2(context);
                }
                else
                {
                    await ValidStep(context);
                }
            }
        }
        public async Task ValidStep2(IDialogContext context)
        {
            await context.SayAsync(text: "Kindly mention a valid step to repeat from(1, 2, 3, or 4)", speak: "Mention the steps to repeat", options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(StepReceived2);
        }
        public async Task StepReceived2(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var step = await result;
            foreach (var i in step.Text.ToCharArray())
            {
                if (char.IsNumber(i) && i <= '4')
                {
                    list.Add(i - 48);
                }
            }
            if (list.Count == 1)
            {
                await context.SayAsync(text: "No valid step entered.", speak: "No valid step entered.");
                await ValidStep2(context);
            }
            else
            {
                for (int i = list[1]; i < steptext.Count; i++)
                {
                    if (i == 1)
                    {
                        await context.SayAsync(text: steptext[i], speak: s);
                    }
                    else
                    {
                        await context.SayAsync(text: steptext[i], speak: steptext[i]);
                        //await new GlobalHandler().makessml(context, steptext[i]);
                    }
                    //await new GlobalHandler().makessml(context, steptext[i]);
                }
                await Progress(context);
            }
        }
        public async Task ValidStep(IDialogContext context)
        {
            await context.SayAsync(text: "Kindly mention valid steps to repeat (1, 2, 3, or 4)", speak: "Mention the steps to repeat", options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(StepReceived);
        }
        public async Task StepReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var steps = await result;
            
            foreach(var i in steps.Text.ToCharArray())
            {
                if (char.IsNumber(i) && i<='4')
                {
                    list.Add(i-48);
                }
            }
            if (list.Count == 1)
            {
                await context.SayAsync(text: "No valid step entered.", speak: "No valid step entered.");
                await ValidStep(context);
            }
            else
            {
                for (int i = 1; i < list.Count; i++)
                {
                    if (list[i] == 1)
                    {
                        await context.SayAsync(text: steptext[list[i]], speak: s);
                    }
                    else
                    {
                        await context.SayAsync(text: steptext[list[i]], speak: steptext[list[i]]);
                    }
                    //await new GlobalHandler().makessml(context, steptext[list[i]]);
                }
                await Progress(context);
            }
        }
        public async Task Progress(IDialogContext context)
        {
            //await context.SayAsync(text: "Please confirm when ready for next step.", speak: "Please confirm when ready for next step.");
            //List<string> proceedMessage = con.proceedMessage;
            List<string> proceed = con.proceed;
            //string prompt = new GlobalHandler().GetRandomString(proceedMessage);
            string prompt = "Are you ready to proceed?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: proceed, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, new ConfirmReady().ResumeConfirm, promptOptions);
        }
    }
}
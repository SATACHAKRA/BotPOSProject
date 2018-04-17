using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class CreateLMSTicket : BasicLuisDialog, IDialog<object>
    {
        static GlobalHandler.LMS lms = new GlobalHandler.LMS();
        string f = lms.firstname;
        string fr = lms.freceive;
        string l = lms.lastname;
        string lr = lms.lreceive;
        string ph = lms.phonenumber;
        string phr = lms.phreceive;
        string str = lms.storenumber;
        string strr = lms.strreceive;
        string petxt = lms.previouseidtext;
        string pespk = lms.previouseidspeak;
        string petxtr = lms.ptxtreceive;
        string pespkr = lms.pspkreceive;
        string netxt = lms.neweidtext;
        string nespk = lms.neweidspeak;
        string netxtr = lms.ntxtreceive;
        string nespkr = lms.nspkreceive;
        string pos = lms.position;
        string posr = lms.posreceive;
        string complete = lms.fcom;
        string pf = lms.pfirstname;
        string pl = lms.plastname;
        string pph = lms.pphone;
        string pstr = lms.pstore;
        string ppetxt = lms.ppetxt;
        string ppespk = lms.ppespk;
        string pnetxt = lms.pnetxt;
        string pnespk = lms.pnespk;
        string ppos = lms.ppos;
        List<string> choices = new List<string> { "Yes", "No" };
        public List<string> option = new List<string> { "Redo the form fill up", "Change any field", "Cancel the operation" };
        public List<string> details = new List<string> { "First Name", "Last Name", "Phone Number", "Store Number", "Previous EID", "New EID", "Position", "No change" };
        public string FirstName;
        public string LastName;
        public string PhoneNumber;
        public string phone;
        public string StoreNumber;
        public string Store;
        public string Previouseid;
        public string PreviousEID;
        public string Neweid;
        public string NewEID;
        public string Position;
        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync(text: "A ticket needs to be created to send to LMS.", speak: "A ticket needs to be created to send to LMS.");
            await context.SayAsync(text: "In order to create a ticket you will be required to provide some information.", speak: "In order to create a ticket you will be required to provide some information.");
            string prompt = "Are you ready to Proceed?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, Submission, promptOptions);
        }
        public async Task Submission(IDialogContext context, IAwaitable<string> result)
        {
            string choice = await result;
            if (choice.ToLower() == "yes")
            {
                await First(context);
            }
            else
            {
                await context.SayAsync(text: "You have cancelled the operation. You can again start a New conversation by asking me things like 'Error in POS Open', 'EID Merge', etc", speak: "You have cancelled the operation. You can again start a new conversation by asking me things like 'Error in P O S open', 'e i d merge', etc");
                context.Wait(this.MessageReceived);
            }
        }
        public async Task First(IDialogContext context)
        {
            await context.SayAsync(text: f, speak: f, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(FirstNameReceived);
        }
        public async Task FirstNameReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var FN = await activity;
            this.FirstName = FN.Text;
            await context.SayAsync(text: fr + this.FirstName, speak: fr + this.FirstName);
            await Last(context);
        }
        public async Task Last(IDialogContext context)
        {
            await context.SayAsync(text: l, speak: l, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(LastNameReceived);
        }
        public async Task LastNameReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var LN = await activity;
            this.LastName = LN.Text;
            await context.SayAsync(text: lr + this.LastName, speak: lr + this.LastName);
            await Phone(context);
        }
        public async Task Phone(IDialogContext context)
        {
            await context.SayAsync(text: ph, speak: ph, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(PhoneNumberReceived);
        }
        public async Task PhoneNumberReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var PH = await activity;
            int flag = 0;
            foreach (var c in PH.Text.ToCharArray())
            {
                if (!char.IsNumber(c) && c!='-')
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                await context.SayAsync(text: "Phone number cannot contain any characters. Try again...", speak: "Phone number cannot contain any character. Try again");
                await Phone(context);
            }
            else
            {
                if (PH.Text.Replace("-","").Length!=10)
                {
                    await context.SayAsync(text: "Phone number should be of length 10. Try again...", speak: "Phone number should be of length 10. Try again...");
                    await Phone(context);
                }
                else
                {
                    this.PhoneNumber = PH.Text.Replace("-","");
                    StringBuilder sb = new StringBuilder();
                    foreach(var i in this.PhoneNumber.ToCharArray())
                    {
                        sb.Append(" ").Append(i).Append(" ");
                    }
                    this.phone = sb.ToString().Trim();
                    await context.SayAsync(text: phr + this.PhoneNumber, speak: phr + this.phone);
                    await context.SayAsync(text: str, speak: str, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
                    context.Wait(StoreNumberReceived);
                }
            }
        }
        public async Task StoreNumberReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var SN = await activity;
            this.StoreNumber = SN.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.StoreNumber.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            this.Store = sb.ToString().Trim();
            await context.SayAsync(text: strr+this.StoreNumber, speak: strr + this.Store);
            await context.SayAsync(text: petxt, speak: pespk, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(PeidReceived);
        }
        public async Task PeidReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var PE = await activity;
            this.Previouseid = PE.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.Previouseid.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            this.PreviousEID = sb.ToString().Trim();
            await context.SayAsync(text: petxtr+this.Previouseid, speak: pespkr + this.PreviousEID);
            await context.SayAsync(text: netxt, speak: nespk, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(NeidReceived);
        }
        public async Task NeidReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var NE = await activity;
            this.Neweid = NE.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.Neweid.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            this.NewEID = sb.ToString().Trim();
            await context.SayAsync(text: netxtr+this.Neweid, speak: nespkr + this.NewEID);
            await context.SayAsync(text: pos, speak: pos, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(PositionReceived);
        }
        public async Task PositionReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var POS = await activity;
            this.Position = POS.Text;
            await context.SayAsync(text: posr+this.Position, speak: posr + this.Position);
            await context.SayAsync(text: complete, speak: complete);
            await ShowDetails(context);
        }
        public async Task ShowDetails(IDialogContext context)
        {
            await context.SayAsync(text: "Your details are as follows:<br/>First Name: "+this.FirstName+"."+"<br/>Last Name: "+this.LastName+"."+"<br/>Phone Number: "+this.PhoneNumber+"."+"<br/>Store Number: "+this.StoreNumber+"."+"<br/>Previous EID: "+this.Previouseid+"."+"<br/>New EID: "+this.Neweid+"."+"<br/>Position: "+this.Position+".", speak: "Your details are as follows: First name: " + this.FirstName + ". Last name: " + this.LastName + ". Phone number: " + this.phone + ". Store number: " + this.Store + ". Previous e i d: " + this.PreviousEID + ". New e i d: " + this.NewEID + ". Position: " + this.Position);
            string prompt = "Do you want to Proceed?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, SubmissionForm, promptOptions);
        }
        public async Task SubmissionForm(IDialogContext context, IAwaitable<string> result)
        {
            string choice = await result;
            if (choice.ToLower() == "yes")
            {
                await context.SayAsync(text: "Your message was registered.", speak: "Your message was registered.");
                await context.SayAsync(text: "Once we resolve it, We will get back to you.", speak: "Once we resolve it, we will get back to you.");

                string prompt = "Do you want to Merge another employee?";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, Confirmed, promptOptions);
            }
            else
            {
                string prompt = "Please select one of the following options.";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: option, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, SubmissionNotComplete, promptOptions);
            }
        }
        public async Task SubmissionNotComplete(IDialogContext context, IAwaitable<string> result)
        {
            string option = await result;
            if (option.ToLower().Contains("change"))
            {
                string prompt = "Okay, Tell me which field do you want to change?";
                string retryprompt = "Please Try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: details, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, Change, promptOptions);
            }
            else if (option.ToLower().Contains("cancel"))
            {
                await context.SayAsync(text: "You have cancelled the operation", speak: "You have cancelled the operation.");
                await context.SayAsync(text: "You can again start a new conversation by asking me things like 'POS Error', 'EID Merge', etc.", speak: "You can again start a new conversation by asking me things like 'P O S Error', 'E I D Merge', etc", options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
                context.Wait(this.MessageReceived);
            }
            else if (option.ToLower().Contains("redo"))
            {
                await context.SayAsync(text: "You requested to start the form fill up again", speak: "You requested to start the form fill up again.");
                await First(context);
            }
        }
        public async Task Change(IDialogContext context, IAwaitable<string> result)
        {
            string details = await result;
            if (details.ToLower() == "first name")
            {
                await context.SayAsync(pf + this.FirstName, speak: pf + this.FirstName);
                await FirstRepeat(context);
            }
            else if (details.ToLower() == "last name")
            {
                await context.SayAsync(pl+this.LastName, speak: pl + this.LastName);
                await LastRepeat(context);
            }
            else if (details.ToLower() == "phone number")
            {
                await context.SayAsync(pph+this.PhoneNumber, speak: pph + this.phone);
                await PhoneRepeat(context);
            }
            else if (details.ToLower() == "store number")
            {
                await context.SayAsync(pstr+this.StoreNumber, speak: pstr + this.Store);
                await context.SayAsync(text: str, speak: str, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
                context.Wait(StoreNumberRepeat);
            }
            else if (details.ToLower().Contains("previous"))
            {
                await context.SayAsync(ppetxt+this.Previouseid, speak: ppespk + this.PreviousEID);
                await context.SayAsync(text: petxt, speak: pespk, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
                context.Wait(PEIDRepeat);
            }
            else if (details.ToLower().Contains("new"))
            {
                await context.SayAsync(pnetxt+this.Neweid, speak: pnespk + this.NewEID);
                await context.SayAsync(text: netxt, speak: nespk, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
                context.Wait(NEIDRepeat);
            }
            else if (details.ToLower() == "position")
            {
                await context.SayAsync(ppos+this.Position, speak: ppos+this.Position);
                await context.SayAsync(text: pos, speak: pos, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
                context.Wait(PositionRepeat);
            }
            else
            {
                await ShowDetails(context);
            }
        }
        public async Task FirstRepeat(IDialogContext context)
        {
            await context.SayAsync(text: f, speak: f, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(FirstNameRepeat);
        }
        public async Task FirstNameRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string fname = this.FirstName;
            var fn = await activity;
            this.FirstName = fn.Text;
            await context.SayAsync(text: $"First name is changed form {fname} to {this.FirstName}.", speak: $"First name is changed form {fname} to {this.FirstName}.");
            string prompt = "Do you want to change any other fields?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task LastRepeat(IDialogContext context)
        {
            await context.SayAsync(text: l, speak: l, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(LastNameRepeat);
        }
        public async Task LastNameRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string lname = this.LastName;
            var ln = await activity;
            this.LastName = ln.Text;
            await context.SayAsync(text: $"Last name is changed from {lname} to {this.LastName}.", speak: $"Last name is changed from {lname} to {this.LastName}.");
            string prompt = "Do you want to change any other field?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task PhoneRepeat(IDialogContext context)
        {
            await context.SayAsync(text: ph, speak: ph, options: new MessageOptions() { InputHint = InputHints.ExpectingInput });
            context.Wait(PhoneNumberRepeat);
        }
        public async Task PhoneNumberRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string phone = this.PhoneNumber;
            string phn = this.phone;
            var ph = await activity;
            int flag = 0;
            foreach (var c in ph.Text.ToCharArray())
            {
                if (!char.IsNumber(c) && c != '-')
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                await context.SayAsync(text: "Phone number cannot contain any characters. Try again...", speak: "Phone number cannot contain any character. Try again");
                await Phone(context);
            }
            else
            {
                if (ph.Text.Replace("-", "").Length != 10)
                {
                    await context.SayAsync(text: "Phone number should be of length 10. Try again...", speak: "Phone number should be of length 10. Try again...");
                    await Phone(context);
                }
                else
                {
                    this.PhoneNumber = ph.Text.Replace("-","");
                    StringBuilder sb = new StringBuilder();
                    foreach (var i in this.PhoneNumber.ToCharArray())
                    {
                        sb.Append(" ").Append(i).Append(" ");
                    }
                    this.phone = sb.ToString().Trim();
                    await context.SayAsync(text: $"Phone number is changed from {phone} to {this.PhoneNumber}.", speak: $"Phone number is changed form {phn} to {this.phone}.");
                    string prompt = "Do you want to change any other field?";
                    string retryprompt = "Please try again";
                    var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                    PromptDialog.Choice(context, ConfirmChange, promptOptions);
                }
            }
        }
        public async Task StoreNumberRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string store = this.StoreNumber;
            string str = this.Store;
            var st = await activity;
            this.StoreNumber = st.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.StoreNumber.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            this.Store = sb.ToString().Trim();
            await context.SayAsync(text: $"Store number is changed form {store} to {this.StoreNumber}.", speak: $"Store number is changed form {str} to {this.Store}.");
            string prompt = "Do you want to change any other field?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task PEIDRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string peid = this.Previouseid;
            string pEID = this.PreviousEID;
            var pe = await activity;
            this.Previouseid = pe.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.Previouseid.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            this.PreviousEID = sb.ToString().Trim();
            await context.SayAsync(text: $"Previous EID is changed from {peid} to {this.Previouseid}.", speak: $"Previous e i d is changed from {pEID} to {this.PreviousEID}");

            string prompt = "Do you want to change any other field?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task NEIDRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string neid = this.Neweid;
            string nEID = this.NewEID;
            var ne = await activity;

            this.Neweid = ne.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.Neweid.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            this.NewEID = sb.ToString().Trim();
            await context.SayAsync(text: $"New EID is changed from {neid} to {this.Neweid}.", speak: $"New e i d is changed from {nEID} to {this.NewEID}");

            string prompt = "Do you want to change any other field?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task PositionRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string position = this.Position;
            var pos = await activity;
            this.Position = pos.Text;
            await context.SayAsync(text: $"Position is changed from {position} to {this.Position}.", speak: $"Position is changed form {position} to {this.Position}.");
            string prompt = "Do you want to change any other field?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task ConfirmChange(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower() == "yes")
            {
                string prompt = "Okay, what would you like to change?";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: details, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, Change, promptOptions);
            }
            else
            {
                await ShowDetails(context);
            }
        }
        public async Task Confirmed(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower() == "yes")
            {
                
                await new CreateLMSTicket().StartAsync(context);
            }
            else
            {
                var ticketNumber = "L"+new Random().Next(1000,9999);
                //await context.SayAsync(text: "A LMSTicket is provided to you and your message has been registered.", speak: "L M S Ticket is provided to you and your message has been registered.");

                await new CreateServiceRequest().Start(context, ticketNumber);

            }
        }
    }
}
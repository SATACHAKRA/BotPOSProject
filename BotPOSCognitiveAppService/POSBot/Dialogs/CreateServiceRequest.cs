using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class CreateServiceRequest
    {
        public async Task Start(IDialogContext context, string incident)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in incident.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            string ticket = sb.ToString().Trim();
            if (incident != string.Empty)
            {
                await context.SayAsync(text: "An incident has been created for you. Your incident number is: " + incident, speak: "An incident has been created for you. Your incident number is " + ticket);
            }
            await new CloseContact().StartAsync(context);
        }
    }
}
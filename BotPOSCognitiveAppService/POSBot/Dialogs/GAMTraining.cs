using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace POSBot
{
    public class GAMTraining : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            string gam = "For training, instructions, and additional information on Global Account Manager, please see the Global Account Manager Launch and Learn presentation on Access, or contact GAM Support at GlobalAMSupport@us.com";
            await context.SayAsync(text: gam, speak: gam);
            await context.SayAsync(text: "Press this link to see the presentation: https://www.canva.com", speak: "Press this link to see the presentation");
            await context.SayAsync(text: "Press this link to contact us: https://www.canva.com", speak: "Press this link to contact us");
            var incidentNumber = "E" + new Random().Next(1000, 9999);
            await new CreateServiceRequest().Start(context, incidentNumber);
        }
    }
}
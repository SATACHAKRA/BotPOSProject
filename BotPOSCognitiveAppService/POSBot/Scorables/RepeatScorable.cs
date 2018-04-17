using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POSBot
{
    public class RepeatScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;

        public RepeatScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;
            var dialog = task.Frames.ElementAt(1).Method;
            var cls = dialog.Name;
            if ((cls.Equals("ResumeConfirm") || cls.Equals("Level") || cls.Equals("InfoResume")) && (message != null && !string.IsNullOrWhiteSpace(message.Text)))
            {
                var msg = message.Text.ToLowerInvariant();

                if (msg.ToLower().Equals("repeat") || msg.ToLower().Contains("repeat step") || msg.ToLower().Contains("repeat from step") || msg.ToLower().Contains("continue from step"))
                {
                    return message.Text;
                }
            }

            return null;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;
            var dialog = task.Frames.ElementAt(1).Method;
            var cls = dialog.Name;
            if (message != null)
            {
                if (cls.Equals("ResumeConfirm"))
                {
                    if (message.Text.ToLower().Equals("repeat"))
                    {
                        var con = new ConfirmReady();
                        var interruption = con.Void<object, IMessageActivity>();

                        this.task.Call(interruption, null);

                        await this.task.PollAsync(token);
                    }
                    else if (message.Text.ToLower().Contains("repeat step") || message.Text.ToLower().Contains("repeat from step") || message.Text.ToLower().Contains("continue from step"))
                    {
                        var pstep = new POSSteps(message);
                        var interruption = pstep.Void<object, IMessageActivity>();

                        this.task.Call(interruption, null);

                        await this.task.PollAsync(token);
                    }

                }
                else if (cls.Equals("Level"))
                {
                    var pra = new PersonAssist();
                    var interruption = pra.Void<object, IMessageActivity>();

                    this.task.Call(interruption, null);

                    await this.task.PollAsync(token);
                }
                else if (cls.Equals("InfoResume"))
                {
                    var pri = new PersonInfo();
                    var interruption = pri.Void<object, IMessageActivity>();

                    this.task.Call(interruption, null);

                    await this.task.PollAsync(token);
                }
                
               
            }
        }
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
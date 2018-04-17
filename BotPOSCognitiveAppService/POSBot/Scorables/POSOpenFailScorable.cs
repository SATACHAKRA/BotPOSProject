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
    public class POSOpenFailScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;

        public POSOpenFailScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;
            var dialog = task.Frames.First().Method;
            var cls = dialog.DeclaringType;
            if (!cls.Equals(typeof(RootDialog)) && (message != null && !string.IsNullOrWhiteSpace(message.Text)))
            {
                var msg = message.Text.ToLowerInvariant();

                if (msg.ToLower().Equals("pos") || msg.ToLower().Contains("point") || msg.ToLower().Equals("ped") || msg.ToLower().Contains("cashless") || msg.ToLower().Contains("open fail"))
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

            if (message != null)
            {
                var pos = new POSOpenFail();

                var interruption = pos.Void<object, IMessageActivity>();

                this.task.Call(interruption, null);

                await this.task.PollAsync(token);
            }
        }
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
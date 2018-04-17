using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using System.Threading;
using System.Threading.Tasks;

namespace POSBot
{
    public class TransferToAPersonScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;

        public TransferToAPersonScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                var msg = message.Text.ToLowerInvariant();

                if (msg.ToLower().Contains("transfer") || (msg.ToLower().Contains("talk")) || msg.ToLower().Contains("speak") || msg.ToLower().Contains("technician"))
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
                var tran = new TransferToAPerson();

                var interruption = tran.Void<object, IMessageActivity>();

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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CronNET
{
    public interface ICronDaemon
    {
        void Add(ICronJob job);
        void Remove(ICronJob job);
        void Remove(string name);
        void Clear();
        void Start(CancellationToken cancellationToken);
        Task RunAsync(Func<Task> func, CancellationToken cancellationToken, string name);
        void Stop();

        event EventHandler<string> JobExecuting;
        event EventHandler<string> JobExecuted;
    }
}

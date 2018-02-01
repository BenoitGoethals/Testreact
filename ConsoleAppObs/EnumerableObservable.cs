using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppObs
{
    public sealed class EnumerableObservable : IObservable<string>, IDisposable
    {
        private readonly IEnumerable<string> enumerable;

        public EnumerableObservable(IEnumerable<string> enumerable)
        {
            this.enumerable = enumerable;
            this.cancellationSource = new CancellationTokenSource();
            this.cancellationToken = cancellationSource.Token;
            this.workerTask = Task.Factory.StartNew(() =>
            {
                foreach (var value in this.enumerable)
                {
                    //if task cancellation triggers, raise the proper exception
                    //to stop task execution
                    cancellationToken.ThrowIfCancellationRequested();
                    foreach (var observer in observerList)
                        observer.OnNext(value);
                }
            }, this.cancellationToken);
        }

        //the cancellation token source for starting stopping
        //inner observable working thread
        private readonly CancellationTokenSource cancellationSource;

        //the cancellation flag
        private readonly CancellationToken cancellationToken;

        //the running task that runs the inner running thread
        private readonly Task workerTask;

        //the observer list
        private readonly List<IObserver<string>> observerList = new List<IObserver<string>>();

        public IDisposable Subscribe(IObserver<string> observer)
        {
            observerList.Add(observer);
            //subscription lifecycle missing
            //for readability purpose
            return null;
        }

        public void Dispose()
        {
            //trigger task cancellation
            //and wait for acknoledge
            if (!cancellationSource.IsCancellationRequested)
            {
                cancellationSource.Cancel();
                while (!workerTask.IsCanceled)
                    Thread.Sleep(100);
            }
            cancellationSource.Dispose();
            workerTask.Dispose();
            foreach (var observer in observerList)
                observer.OnCompleted();
        }
    }
}

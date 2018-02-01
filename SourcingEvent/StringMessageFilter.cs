using System;
using System.Collections.Generic;

namespace SourcingEvent
{
    /// <summary>
    /// The filtering observable/observer
    /// </summary>
    public sealed class StringMessageFilter : IObservable<string>, IObserver<string>, IDisposable
    {
        private readonly string filter;

        public StringMessageFilter(string filter)
        {
            this.filter = filter;
        }

        //the observer collection
        private readonly List<IObserver<string>> observerList = new
            List<IObserver<string>>();

        public IDisposable Subscribe(IObserver<string> observer)
        {
            this.observerList.Add(observer);
            return null;
        }

        //a simple implementation
        //that disables message routing once
        //the OnCompleted has been invoked
        private bool hasCompleted = false;

        public void OnCompleted()
        {
            hasCompleted = true;
            foreach (var observer in observerList)
                observer.OnCompleted();
        }

        //routes error messages until not completed
        public void OnError(Exception error)
        {
            if (!hasCompleted)
                foreach (var observer in observerList)
                    observer.OnError(error);
        }

        //routes valid messages until not completed
        public void OnNext(string value)
        {
            Console.WriteLine("Filtering {0}", value);
            if (!hasCompleted &&
                value.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                foreach (var observer in observerList)
                    observer.OnNext(value);
        }

        public void Dispose()
        {
            OnCompleted();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace SourcingEvent
{
    public sealed class FileNameMessageCorrelator : IObservable<string>, IObserver<string>, IDisposable
    {
        private readonly Func<string, string> correlationKeyExtractor;

        public FileNameMessageCorrelator(Func<string, string>
            correlationKeyExtractor)
        {
            this.correlationKeyExtractor = correlationKeyExtractor;
        }

        //the observer collection
        private readonly List<IObserver<string>> observerList = new
            List<IObserver<string>>();

        public IDisposable Subscribe(IObserver<string> observer)
        {
            this.observerList.Add(observer);
            return null;
        }

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


        //multiple strings per each key
        private readonly NameValueCollection correlations = new NameValueCollection();

        //routes valid messages until not completed
        public void OnNext(string value)
        {
            if (hasCompleted) return;
            //check if subscriber has completed
            Console.WriteLine("Parsing message: {0}", value);
            //try extracting the correlation ID
            var correlationID = correlationKeyExtractor(value);
            //check if the correlation is available
            if (correlationID == null) return;
            //append the new file name to the correlation state
            correlations.Add(correlationID, value);
            //in this example we will consider always
            //correlations of two items
            if (correlations.GetValues(correlationID).Count() == 2)
            {
                //once the correlation is complete
                //read the two files and push the
                //two contents altogether to the
                //observers
                var fileData = correlations.GetValues(correlationID)
                    //route messages to the ReadAllText method
                    .Select(File.ReadAllText)
                    //materialize the query
                    .ToArray();
                var newValue = string.Join("|", fileData);
                foreach (var observer in observerList)
                    observer.OnNext(newValue);
                correlations.Remove(correlationID);
            }
        }

        public void Dispose()
        {
        }
    }
}
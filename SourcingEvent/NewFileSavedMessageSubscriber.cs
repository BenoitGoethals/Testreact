using System;

namespace SourcingEvent
{
    public class NewFileSavedMessageSubscriber : IObserver<string>
    {
        public NewFileSavedMessageSubscriber()
        {
        }
        public void OnCompleted()
        {
            Console.WriteLine("-> END");
        }
        public void OnError(Exception error)
        {
            Console.WriteLine("-> {0}", error.Message);
        }
        public void OnNext(string value)
        {
            Console.WriteLine("-> {0}", value);
        }
    }
}
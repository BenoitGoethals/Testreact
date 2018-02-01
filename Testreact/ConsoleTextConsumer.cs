using System;

namespace Testreact
{
    internal class ConsoleTextConsumer : IObserver<string>
    {
        private bool finished = false;
        public void OnCompleted()
        {
            if (finished)
            {
                OnError(new Exception("This consumer already finished it's lifecycle"));
                return;
            }
            finished = true;
            Console.WriteLine("<- END");
        }
        public void OnError(Exception error)
        {
            Console.WriteLine("<- ERROR");
            Console.WriteLine("<- {0}", error.Message);
        }
        public void OnNext(string value)
        {
            if (finished)
            {
                OnError(new Exception("This consumer finished its lifecycle"));
                return;
            }
            //shows the received message
            Console.WriteLine("-> {0}", value);
            //do something
            //ack the caller
            Console.WriteLine("<- OK");
        }
    }
}
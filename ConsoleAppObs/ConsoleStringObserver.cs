using System;

namespace ConsoleAppObs
{
    internal class ConsoleStringObserver : IObserver<string>
    {
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourcingEvent;

namespace Testreact
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Watching for new files");
            using (var publisher = new NewFileSavedMessagePublisher(@"[WRITE A PATH HERE]"))
            using (var filter = new StringMessageFilter(".txt"))
            {
                //subscribe the filter to publisher messages
                publisher.Subscribe(filter);
                //subscribe the console subscriber to the filter
                //instead that directly to the publisher
                filter.Subscribe(new NewFileSavedMessageSubscriber());
                Console.WriteLine("Press RETURN to exit");
                Console.ReadLine();

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SourcingEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var publisher = new NewFileSavedMessagePublisher(@"[WRITE A PATH HERE]"))
                //creates a new correlator by specifying the correlation key
                //extraction function made with a Regular expression that
                //extract a file ID similar to FILEID0001
            using (var correlator = new FileNameMessageCorrelator(ExtractCorrelationKey))
            {
                //subscribe the correlator to publisher messages
                publisher.Subscribe(correlator);
                //subscribe the console subscriber to the correlator
                //instead that directly to the publisher
                correlator.Subscribe(new NewFileSavedMessageSubscriber());
                //wait for user RETURN
                Console.ReadLine();
            }
        }

        private static string ExtractCorrelationKey(string arg)
        {
            var match = Regex.Match(arg, "(FILEID\\d{4})");
            if (match.Success)
                return match.Captures[0].Value;
            else
                return null;
        }
    }
}

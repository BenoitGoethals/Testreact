using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppObs
{
    class Program
    {
        static void Main(string[] args)
        {
            //the observable collection
            var collection = new ObservableCollection<string>();
            using (var observable = new NotifiableCollectionObservable(collection))
            using (var observer = observable.Subscribe(new ConsoleStringObserver()))
            {
                collection.Add("ciao");
                collection.Add("hahahah");
                collection.Insert(0, "new first line");
                collection.RemoveAt(0);
                Console.WriteLine("Press RETURN to EXIT");
                Console.ReadLine();
            }
        }
        private static void OnCollectionChanged(object sender,NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<string>;
            if (e.NewStartingIndex >= 0) //adding new items
                Console.WriteLine("-> {0} {1}", e.Action,collection[e.NewStartingIndex]);
            else //removing items
                Console.WriteLine("-> {0} at {1}", e.Action, e.OldStartingIndex);
        }
    }
}

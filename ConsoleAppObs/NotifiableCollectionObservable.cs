using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ConsoleAppObs
{
    public sealed class NotifiableCollectionObservable : IObservable<string>,
        IDisposable
    {
        private readonly ObservableCollection<string> collection;

        public NotifiableCollectionObservable(ObservableCollection<string>
            collection)
        {
            this.collection = collection;
            this.collection.CollectionChanged += collection_CollectionChanged;
        }

        private void collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

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
            this.collection.CollectionChanged -= collection_CollectionChanged;
            foreach (var observer in observerList)
                observer.OnCompleted();
        }
    }
}

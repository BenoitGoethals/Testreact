using System;
using System.Collections.Generic;
using System.IO;

namespace SourcingEvent
{
    public sealed class NewFileSavedMessagePublisher : IObservable<string>,
        IDisposable
    {
        private readonly FileSystemWatcher _watcher;

        public NewFileSavedMessagePublisher(string path)
        {
            //creates a new file system event router
            this._watcher = new FileSystemWatcher(path);
            //register for handling File Created event
            this._watcher.Created += OnFileCreated;
            //enable event routing
            this._watcher.EnableRaisingEvents = true;
        }

        //signal all observers a new file arrived
        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            foreach (var observer in subscriberList)
                observer.OnNext(e.FullPath);
        }

        //the subscriber list
        private readonly List<IObserver<string>> subscriberList = new List<IObserver<string>>();

        public IDisposable Subscribe(IObserver<string> observer)
        {
            //register the new observer
            subscriberList.Add(observer);
            return null;
        }

        public void Dispose()
        {
            //disable file system event routing
            this._watcher.EnableRaisingEvents = false;
            //deregister from watcher event handler
            this._watcher.Created -= OnFileCreated;
            //dispose the watcher
            this._watcher.Dispose();
            //signal all observers that job is done
            foreach (var observer in subscriberList)
                observer.OnCompleted();
        }
    }
}
using System;
using System.Collections.Generic;

namespace WPFGamesCollection
{
    public class Subject<T>
    {
        private List<T> observersList;
        public List<T> ObserversList => observersList;

        public Subject() 
        {
            observersList = new List<T>();
        }

        public void AddObserver(T observer)
        {
            if (observersList.Contains(observer)) throw new Exception("Error: this list of observers already contains this observer");
            else observersList.Add(observer);
        }

        public void RemoveObserver(T observer)
        {
            if (observersList.Contains(observer)) observersList.Remove(observer);
            else throw new Exception("Error: this list of observers doesn't contain this observer");
        }
    }
}

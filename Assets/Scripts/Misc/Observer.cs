using System.Collections.Generic;

public interface IObserver
{
    void OnNotify(EventType eventType);
}

public class Subject
{
    private List<IObserver> observers = new List<IObserver>();

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void UnregisterObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers(EventType eventType)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(eventType);
        }
    }
}

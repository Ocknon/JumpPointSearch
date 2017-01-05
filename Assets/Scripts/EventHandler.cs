using UnityEngine;
using System;
using System.Collections.Generic;

public class EventHandler
{
    private static EventHandler _instance = null;
    public static EventHandler Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EventHandler();

            return _instance;
        }
    }

    public delegate void EventDelegate<T>(T e) where T : Event;
    private Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

    public void Subscribe<T>(EventDelegate<T> del) where T : Event
    {
        Delegate _tempDel;
        _delegates[typeof(T)] = (_delegates.TryGetValue(typeof(T), out _tempDel)) ? Delegate.Combine(del, _tempDel) : del;
    }

    public void Unsubscribe<T>(EventDelegate<T> del) where T : Event
    {
        Delegate _tempDel;
        if (_delegates.TryGetValue(typeof(T), out _tempDel)) 
        {
            _tempDel = Delegate.Remove(_delegates[typeof(T)], del);

            if (_tempDel == null)
                _delegates.Remove(typeof(T));
            else
                _delegates[typeof(T)] = _tempDel;
        }
    }

    public void Broadcast(Event e)
    {
        if (e == null)
            return;

        Delegate _tempDel;
        if (_delegates.TryGetValue(e.GetType(), out _tempDel))
        {
            _tempDel.DynamicInvoke(e);
        }
    }
}

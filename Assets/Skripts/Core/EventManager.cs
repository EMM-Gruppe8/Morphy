using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Simulation class implements the discrete event simulator pattern.
/// Events are pooled, with a default capacity of 4 instances.
/// </summary>
public static class EventManager
{
    static HeapQueue<Event> eventQueue = new HeapQueue<Event>();
    static Dictionary<System.Type, Stack<Event>> eventPools = new Dictionary<System.Type, Stack<Event>>();

    /// <summary>
    /// Create a new event of type T and return it, but do not schedule it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    static public T New<T>() where T : Event, new()
    {
        Stack<Event> pool;
        if (!eventPools.TryGetValue(typeof(T), out pool))
        {
            pool = new Stack<Event>(4);
            pool.Push(new T());
            eventPools[typeof(T)] = pool;
        }

        if (pool.Count > 0)
            return (T) pool.Pop();
        else
            return new T();
    }

    /// <summary>
    /// Clear all pending events and reset the tick to 0.
    /// </summary>
    public static void Clear()
    {
        eventQueue.Clear();
    }

    /// <summary>
    /// Schedule an event for a future tick, and return it.
    /// </summary>
    /// <returns>The event.</returns>
    /// <param name="tick">Tick.</param>
    /// <typeparam name="T">The event type parameter.</typeparam>
    static public T Schedule<T>(float tick = 0) where T : Event, new()
    {
        var ev = New<T>();
        ev.tick = Time.time + tick;
        eventQueue.Push(ev);
        return ev;
    }

    /// <summary>
    /// Reschedule an existing event for a future tick, and return it.
    /// </summary>
    /// <returns>The event.</returns>
    /// <param name="tick">Tick.</param>
    /// <typeparam name="T">The event type parameter.</typeparam>
    static public T Reschedule<T>(T ev, float tick) where T : Event, new()
    {
        ev.tick = Time.time + tick;
        eventQueue.Push(ev);
        return ev;
    }

    /// <summary>
    /// Return the simulation model instance for a class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static public T GetModel<T>() where T : class, new()
    {
        return InstanceRegister<T>.instance;
    }

    /// <summary>
    /// Set a simulation model instance for a class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static public void SetModel<T>(T instance) where T : class, new()
    {
        InstanceRegister<T>.instance = instance;
    }

    /// <summary>
    /// Destroy the simulation model instance for a class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static public void DestroyModel<T>() where T : class, new()
    {
        InstanceRegister<T>.instance = null;
    }

    /// <summary>
    /// Tick the simulation. Returns the count of remaining events.
    /// If remaining events is zero, the simulation is finished unless events are
    /// injected from an external system via a Schedule() call.
    /// </summary>
    /// <returns></returns>
    static public int Tick()
    {
        var time = Time.time;
        var executedEventCount = 0;
        while (eventQueue.Count > 0 && eventQueue.Peek().tick <= time)
        {
            var ev = eventQueue.Pop();
            var tick = ev.tick;
            ev.ExecuteEvent();
            if (ev.tick > tick)
            {
                //event was rescheduled, so do not return it to the pool.
            }
            else
            {
                // Debug.Log($"<color=green>{ev.tick} {ev.GetType().Name}</color>");
                ev.Cleanup();
                try
                {
                    eventPools[ev.GetType()].Push(ev);
                }
                catch (KeyNotFoundException)
                {
                    //This really should never happen inside a production build.
                    Debug.LogError($"No Pool for: {ev.GetType()}");
                }
            }

            executedEventCount++;
        }

        return eventQueue.Count;
    }

    /// <summary>
    /// An event is something that happens at a point in time in a simulation.
    /// The Precondition method is used to check if the event should be executed,
    /// as conditions may have changed in the simulation since the event was 
    /// originally scheduled.
    /// </summary>
    /// <typeparam name="Event"></typeparam>
    public abstract class Event : System.IComparable<Event>
    {
        internal float tick;

        public int CompareTo(Event other)
        {
            return tick.CompareTo(other.tick);
        }

        public abstract void Execute();

        public virtual bool Precondition() => true;

        internal virtual void ExecuteEvent()
        {
            if (Precondition())
                Execute();
        }

        /// <summary>
        /// This method is generally used to set references to null when required.
        /// It is automatically called by the Simulation when an event has completed.
        /// </summary>
        internal virtual void Cleanup()
        {
        }
    }

    /// <summary>
    /// Event<T> adds the ability to hook into the OnExecute callback
    /// whenever the event is executed. Use this class to allow functionality
    /// to be plugged into your application with minimal or zero configuration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Event<T> : Event where T : Event<T>
    {
        public static System.Action<T> OnExecute;

        internal override void ExecuteEvent()
        {
            if (Precondition())
            {
                Execute();
                OnExecute?.Invoke((T) this);
            }
        }
    }

    /// <summary>
    /// This class provides a container for creating singletons for any other class,
    /// within the scope of the Simulation. It is typically used to hold the simulation
    /// models and configuration classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static class InstanceRegister<T> where T : class, new()
    {
        public static T instance = new T();
    }
}
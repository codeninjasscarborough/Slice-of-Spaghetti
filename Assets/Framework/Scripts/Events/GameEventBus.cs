using System;
using System.Collections.Generic;

namespace CardGame
{
    public static class GameEventBus
    {
        static readonly Dictionary<Type, List<Delegate>> _handlers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _handlers[type] = list;
            }
            list.Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            if (_handlers.TryGetValue(typeof(T), out var list))
                list.Remove(handler);
        }

        public static void Publish<T>(T evt)
        {
            if (!_handlers.TryGetValue(typeof(T), out var list)) return;

            // Iterate over a snapshot to safely handle unsubscriptions during dispatch
            var snapshot = list.ToArray();
            foreach (var d in snapshot)
                ((Action<T>)d).Invoke(evt);
        }

        public static void ClearAll() => _handlers.Clear();
    }
}

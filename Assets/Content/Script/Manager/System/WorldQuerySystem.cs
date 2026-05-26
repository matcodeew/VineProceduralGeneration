using System;
using System.Collections.Generic;

namespace PVG
{
    public static class WorldQuerySystem
    {
        private static readonly Dictionary<Type, List<IWorldQueryElement>> _elements = new();

        public static void Register<T>(T element) where T : class, IWorldQueryElement
        {
            Type type = typeof(T);

            if (!_elements.TryGetValue(type, out var list))
            {
                list = new List<IWorldQueryElement>();
                _elements.Add(type, list);
            }

            if (!list.Contains(element))
                list.Add(element);
        }

        public static void Unregister<T>(T element) where T : class, IWorldQueryElement
        {
            Type type = typeof(T);

            if (_elements.TryGetValue(type, out var list))
            {
                list.Remove(element);
            }
        }

        public static List<T> GetAll<T>() where T : class, IWorldQueryElement
        {
            Type type = typeof(T);

            if (!_elements.TryGetValue(type, out var list))
                return new List<T>();

            List<T> result = new(list.Count);

            foreach (var e in list)
                result.Add(e as T);

            return result;
        }
    }
}
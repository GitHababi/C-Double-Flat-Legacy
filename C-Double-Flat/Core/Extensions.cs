using System;
using System.Collections.Generic;
using System.Linq;

namespace C_Double_Flat.Core
{
    public static class Extensions
    {
        public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
        {
            List<KeyValuePair<K, V>> pairs = second.ToList();
            pairs.ForEach(pair => first.Add(pair.Key, pair.Value));
        }
        public static void ConsoleWrite<T>(this List<T> first)
        {
            first.ForEach(item => Console.WriteLine(item));
        }
        public static bool MatchesList<T>(this T first, List<T> second)
        {
            foreach (T item in second) if (first.Equals(item)) return true;
            return false;
        }
        public static bool MatchesArray<T>(this T first, T[] second)
        {
            foreach (T item in second) if (first.Equals(item)) return true;
            return false;
        }
    }
}

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace QFetcher.Models
{
    public static class ConcurrentBagExtention
    {
        public static void AddRange<T>(this ConcurrentBag<T> concurrentBag,List<T> listToAdd)
        {
            listToAdd.ForEach(concurrentBag.Add);
        }
    }
}
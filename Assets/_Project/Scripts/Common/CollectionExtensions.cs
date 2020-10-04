using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AntonioHR.Common
{
    public static class CollectionExtensions
    {
        public static T RandomItem<T>(this IEnumerable<T> collection)
        {
            int max = collection.Count();
            
            Debug.Assert(max > 0, "This function expects a collection that is not empty");

            return collection.ElementAt(Random.Range(0, max));
        }
    }
}
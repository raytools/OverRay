﻿using System;
using System.Collections.Generic;

namespace OverRay.Hook.Utils
{
    public static class Locking
    {
        #region IDictionary

        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            lock (dictionary)
            {
                dictionary[key] = value;
            }
        }

        public static void Delete<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            lock (dictionary)
            {
                dictionary.Remove(key);
            }
        }

        #endregion
    }
}
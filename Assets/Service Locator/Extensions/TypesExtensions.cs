using System;
using UnityEngine;

namespace Extensions
{
    public static class TypesExtensions
    {
        public static bool IsMonoBehaviour(this Type t)
        {
            return typeof(MonoBehaviour).IsAssignableFrom(t);
        }
    }
}

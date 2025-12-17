// Assets/Twinspec/Scripts/Runtime/Interop/JsonCompat.cs
using UnityEngine;

namespace Twinspec.Interop
{
    public static class JsonCompat
    {
        public static T FromJson<T>(string json) where T : new()
        {
            // JsonUtility requires fields to exist; keep payloads stable.
            // If payload is missing fields, defaults will remain.
            var obj = new T();
            JsonUtility.FromJsonOverwrite(json, obj);
            return obj;
        }
    }
}
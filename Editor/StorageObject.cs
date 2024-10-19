using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBG.EditorUserSettings.Editor
{
    [Serializable]
    public class KeyValuePair
    {
        public string Key;
        [SerializeReference]
        public object Value;

        public KeyValuePair(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }

    [Serializable]
    internal class KeyValueStringPair
    {
        public string Key;
        public string ValueString;

        public KeyValueStringPair(string key, object stringOrPrimitive)
        {
            Key = key;
            ValueString = stringOrPrimitive?.ToString();
        }

        public TValue GetValue<TValue>()
        {
            if (string.IsNullOrEmpty(ValueString))
            {
                return default;
            }

            return (TValue)Convert.ChangeType(ValueString, typeof(TValue));
        }

        public object GetValue(Type type)
        {
            return Convert.ChangeType(ValueString, type);
        }

        public object GetValue(string typeAssemblyQualifiedName)
        {
            if (string.IsNullOrEmpty(ValueString))
            {
                return default;
            }

            Type type = Type.GetType(typeAssemblyQualifiedName);
            return GetValue(type);
        }
    }

    [Serializable]
    internal class GeneralList
    {
        public string TypeAssemblyQualifiedName;
        public List<KeyValuePair> List;

        public GeneralList(string typeFullName, int capcity)
        {
            TypeAssemblyQualifiedName = typeFullName;
            List = new List<KeyValuePair>(capcity);
        }
    }

    [Serializable]
    internal class PrimitiveList
    {
        public string TypeAssemblyQualifiedName;
        public List<KeyValueStringPair> List;

        public PrimitiveList(string typeFullName, int capcity)
        {
            TypeAssemblyQualifiedName = typeFullName;
            List = new List<KeyValueStringPair>(capcity);
        }
    }

    [Serializable]
    internal class StorageObject
    {
        // For reference types and serializable value types
        [SerializeField]
        public List<GeneralList> GeneralLists = new List<GeneralList>();

        // For string and primitive types : <TypeFullName, <Key - ValueString>[]>[]
        [SerializeField]
        public List<PrimitiveList> PrimitiveLists = new List<PrimitiveList>();


        public void Reload(Dictionary<Type, Dictionary<string, object>> cacheDict)
        {
            GeneralLists.Clear();
            PrimitiveLists.Clear();

            foreach (KeyValuePair<Type, Dictionary<string, object>> typeKvDict in cacheDict)
            {
                Type type = typeKvDict.Key;
                if (type.IsPrimitive || type == typeof(string))
                {
                    PrimitiveList typedKeyValueStringPairRegistry = new PrimitiveList(type.FullName, typeKvDict.Value.Count);
                    PrimitiveLists.Add(typedKeyValueStringPairRegistry);

                    foreach (KeyValuePair<string, object> kv in typeKvDict.Value)
                    {
                        typedKeyValueStringPairRegistry.List.Add(new KeyValueStringPair(kv.Key, kv.Value));
                    }
                }
                else
                {
                    GeneralList typedObjectRegistry = new GeneralList(type.FullName, typeKvDict.Value.Count);
                    GeneralLists.Add(typedObjectRegistry);

                    foreach (KeyValuePair<string, object> kv in typeKvDict.Value)
                    {
                        typedObjectRegistry.List.Add(new KeyValuePair(kv.Key, kv.Value));
                    }
                }
            }
        }

        public Dictionary<Type, Dictionary<string, object>> CreateCacheDict()
        {
            int capcity = GeneralLists.Count + PrimitiveLists.Count;
            Dictionary<Type, Dictionary<string, object>> cacheDict = new Dictionary<Type, Dictionary<string, object>>(capcity);

            foreach (GeneralList generalList in GeneralLists)
            {
                Type type = Type.GetType(generalList.TypeAssemblyQualifiedName);
                Dictionary<string, object> objDict = new Dictionary<string, object>(generalList.List.Count);
                cacheDict.Add(type, objDict);

                foreach (KeyValuePair kv in generalList.List)
                {
                    objDict.Add(kv.Key, kv.Value);
                }
            }

            foreach (PrimitiveList primitiveList in PrimitiveLists)
            {
                Type type = Type.GetType(primitiveList.TypeAssemblyQualifiedName);
                Dictionary<string, object> objDict = new Dictionary<string, object>(primitiveList.List.Count);
                cacheDict.Add(type, objDict);

                foreach (KeyValueStringPair kv in primitiveList.List)
                {
                    objDict.Add(kv.Key, kv.GetValue(type));
                }
            }

            return cacheDict;
        }
    }
}

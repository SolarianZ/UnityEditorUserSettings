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
    }

    [Serializable]
    internal class GeneralList
    {
        public string TypeFullName;
        public List<KeyValuePair> List;

        public GeneralList(string typeFullName, int capcity)
        {
            TypeFullName = typeFullName;
            List = new List<KeyValuePair>(capcity);
        }
    }

    [Serializable]
    internal class PrimitiveList
    {
        public string TypeFullName;
        public List<KeyValueStringPair> List;

        public PrimitiveList(string typeFullName, int capcity)
        {
            TypeFullName = typeFullName;
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
        
        public Dictionary<string, GeneralList> GetGeneralListDict()
        {
            Dictionary<string, GeneralList> dict = new Dictionary<string, GeneralList>(GeneralLists.Count);
            foreach (GeneralList list in GeneralLists)
            {
                dict.Add(list.TypeFullName, list);
            }

            return dict;
        }

        public Dictionary<string, PrimitiveList> GetPrimitiveListDict()
        {
            Dictionary<string, PrimitiveList> dict = new Dictionary<string, PrimitiveList>(PrimitiveLists.Count);
            foreach (PrimitiveList list in PrimitiveLists)
            {
                dict.Add(list.TypeFullName, list);
            }

            return dict;
        }
    }
}

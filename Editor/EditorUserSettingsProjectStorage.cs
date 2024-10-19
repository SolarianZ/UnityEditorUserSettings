using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GBG.EditorUserSettings.Editor
{
    [FilePath("UserSettings/EditorUserSettingsProjectStorage.asset",
        FilePathAttribute.Location.ProjectFolder)]
    internal class EditorUserSettingsProjectStorage :
        ScriptableSingleton<EditorUserSettingsProjectStorage>,
        ISerializationCallbackReceiver
    {
        // For reference types and serializable value types
        [SerializeField]
        private StorageObject _storageObject = new StorageObject();

        // <Type, <Key, Value>>
        private readonly Dictionary<Type, Dictionary<string, object>> _cacheDict = new Dictionary<Type, Dictionary<string, object>>();
        private Dictionary<string, GeneralList> _generalListDict;
        private Dictionary<string, PrimitiveList> _primitiveListDict;


        public bool Has<T>(string key)
        {
            Type type = typeof(T);
            if (_cacheDict.TryGetValue(type, out Dictionary<string, object> objDict))
            {
                return objDict.ContainsKey(key);
            }

            return false;
        }

        public T Get<T>(string key, T defaultValue)
        {
            if (TryGet<T>(key, out T value))
            {
                return value;
            }

            return defaultValue;
        }

        public bool TryGet<T>(string key, out T value)
        {
            Type type = typeof(T);
            if (_cacheDict.TryGetValue(type, out Dictionary<string, object> objDict))
            {
                if (objDict.TryGetValue(key, out object obj))
                {
                    value = (T)obj;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public void Set<T>(string key, T value)
        {
            Type type = typeof(T);
            if (!_cacheDict.TryGetValue(type, out Dictionary<string, object> objDict))
            {
                objDict = new Dictionary<string, object>();
                _cacheDict[type] = objDict;
            }

            objDict[key] = value;

            Save(true);
        }


        #region ISerializationCallbackReceiver

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _storageObject.Reload(_cacheDict);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            //_cacheDict.Clear();

        }

        #endregion
    }
}

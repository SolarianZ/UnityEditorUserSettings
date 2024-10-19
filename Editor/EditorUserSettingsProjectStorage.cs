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
        IEditorUserSettingsStorage,
        ISerializationCallbackReceiver
    {
        // For reference types and serializable value types
        [SerializeField]
        private StorageObject _storageObject = new StorageObject();

        // <Type, <Key, Value>>
        private Dictionary<Type, Dictionary<string, object>> _cacheDict = new Dictionary<Type, Dictionary<string, object>>();

        //private IEqualityComparer<string> _stringComparer;
        //private IEqualityComparer<string> StringComparer
        //{
        //    get
        //    {
        //        _stringComparer ??= EqualityComparer<string>.Default;
        //        return _stringComparer;
        //    }
        //}

        public int BatchingCounter { get; private set; }


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

        public bool Remove<T>(string key)
        {
            Type type = typeof(T);
            if (_cacheDict.TryGetValue(type, out Dictionary<string, object> objDict))
            {
                if (objDict.Remove(key))
                {
                    Save(true);
                    return true;
                }
            }

            return false;
        }

        public bool RemoveAll<T>()
        {
            Type type = typeof(T);
            if (_cacheDict.Remove(type))
            {
                Save(true);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            _cacheDict.Clear();
            Save(true);
        }


        public Batching StartBatching()
        {
            BatchingCounter++;
            return new Batching(this);
        }

        public void EndBatching()
        {
            if (BatchingCounter == 0)
            {
                Debug.LogError("​[Editor User Settings] The project editor user settings are currently not in batching mode.");
                return;
            }

            BatchingCounter--;
            if (BatchingCounter == 0)
            {
                Save(true);
            }
        }

        protected override void Save(bool saveAsText)
        {
            if (BatchingCounter > 0)
            {
                return;
            }

            base.Save(saveAsText);
        }


        #region ISerializationCallbackReceiver

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _storageObject.Reload(_cacheDict);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _cacheDict = _storageObject.CreateCacheDict();
        }

        #endregion
    }
}

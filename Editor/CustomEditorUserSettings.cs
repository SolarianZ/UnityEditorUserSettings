using UnityEditor;
using UnityEngine;

namespace GBG.EditorUserSettings.Editor
{
    public static class CustomEditorUserSettings
    {
        [MenuItem("Tools/Bamboo/Editor User Settings/Inspect Storage Object for Project")]
        internal static void InspectProjectStorageObject()
        {
            Selection.activeObject = (ScriptableObject)GetStorage(false);
        }

        [MenuItem("Tools/Bamboo/Editor User Settings/Show Storage Object for Project in Folder")]
        internal static void ShowProjectStorageObjectInFolder()
        {
            GetStorage(false).ShowInFolder();
        }

        [MenuItem("Tools/Bamboo/Editor User Settings/Inspect Storage Object Shared Across Projects")]
        internal static void InspectSharedStorageObject()
        {
            Selection.activeObject = (ScriptableObject)GetStorage(true);
        }

        [MenuItem("Tools/Bamboo/Editor User Settings/Show Storage Object Shared Across Projects in Folder")]
        internal static void ShowSharedStorageObjectInFolder()
        {
            GetStorage(true).ShowInFolder();
        }

        [MenuItem("Tools/Bamboo/Editor User Settings/Source Code")]
        internal static void OpenSourceCodeWebsite()
        {
            Application.OpenURL("https://github.com/SolarianZ/UnityEditorUserSettings");
        }


        private static IEditorUserSettingsStorage GetStorage(bool isSharedAcrossProjects)
        {
            return isSharedAcrossProjects
                ? (IEditorUserSettingsStorage)EditorUserSettingsSharedStorage.instance
                : (IEditorUserSettingsStorage)EditorUserSettingsProjectStorage.instance;
        }

        private static void TryDestroy(this IEditorUserSettingsStorage storage, bool isSharedAcrossProjects)
        {
            if (isSharedAcrossProjects && storage != null && storage.BatchingCounter == 0)
            {
                storage.Destroy();
            }
        }


        /// <summary>
        /// 检查配置是否存在。
        /// </summary>
        /// <typeparam name="T">配置的类型。注意：此方法只会检查给定的类型，不会自动处理多态。</typeparam>
        /// <param name="key">配置的键。</param>
        /// <param name="isSharedAcrossProjects">配置是否在项目间共享。若为true，则在<see cref="UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder"/>/UserSettings文件夹中查找；否则在项目<see cref="Application.dataPath"/>/../UserSettings文件夹下查找。</param>
        /// <returns>配置是否存在。</returns>
        public static bool Has<T>(string key, bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            bool has = storage.Has<T>(key);
            storage.TryDestroy(isSharedAcrossProjects);

            return has;
        }

        /// <summary>
        /// 获取配置。
        /// </summary>
        /// <typeparam name="T">配置的类型。注意：此方法只会检查给定的类型，不会自动处理多态。</typeparam>
        /// <param name="key">配置的键。</param>
        /// <param name="defaultValue">配置不存在时，将返回此默认值。</param>
        /// <param name="isSharedAcrossProjects">配置是否在项目间共享。若为true，则在<see cref="UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder"/>/UserSettings文件夹中查找；否则在项目<see cref="Application.dataPath"/>/../UserSettings文件夹下查找。</param>
        /// <returns>配置值或defaultValue。</returns>
        public static T Get<T>(string key, T defaultValue, bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            T value = storage.Get<T>(key, defaultValue);
            storage.TryDestroy(isSharedAcrossProjects);

            return value;
        }

        /// <summary>
        /// 尝试获取配置。
        /// </summary>
        /// <typeparam name="T">配置的类型。注意：此方法只会检查给定的类型，不会自动处理多态。</typeparam>
        /// <param name="key">配置的键。</param>
        /// <param name="value">配置的值。</param>
        /// <param name="isSharedAcrossProjects">配置是否在项目间共享。若为true，则在<see cref="UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder"/>/UserSettings文件夹中查找；否则在项目<see cref="Application.dataPath"/>/../UserSettings文件夹下查找。</param>
        /// <returns>是否成功获取到了配置。</returns>
        public static bool TryGet<T>(string key, out T value, bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            bool result = storage.TryGet<T>(key, out value);
            storage.TryDestroy(isSharedAcrossProjects);

            return result;
        }


        /// <summary>
        /// 设置配置。
        /// </summary>
        /// <typeparam name="T">配置的类型。注意：此方法只会检查给定的类型，不会自动处理多态。</typeparam>
        /// <param name="key">配置的键。</param>
        /// <param name="value">配置的值。</param>
        /// <param name="isSharedAcrossProjects">配置是否在项目间共享。若为true，则在<see cref="UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder"/>/UserSettings文件夹中查找；否则在项目<see cref="Application.dataPath"/>/../UserSettings文件夹下查找。</param>
        public static void Set<T>(string key, T value, bool isSharedAcrossProjects = false)
        {
            if (value is Object obj)
            {
                Debug.LogError($"[Editor User Settings] Cannot serialize object '{obj}' that derive from UnityEngine.Object.", obj);
                return;
            }

            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            storage.Set(key, value);
            storage.TryDestroy(isSharedAcrossProjects);
        }


        /// <summary>
        /// 移除指定类型的配置。
        /// </summary>
        /// <typeparam name="T">配置的类型。注意：此方法只会检查给定的类型，不会自动处理多态。</typeparam>
        /// <param name="key">配置的键。</param>
        /// <param name="isSharedAcrossProjects">配置是否在项目间共享。若为true，则在<see cref="UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder"/>/UserSettings文件夹中查找；否则在项目<see cref="Application.dataPath"/>/../UserSettings文件夹下查找。</param>
        /// <returns>是否成功移除了配置。</returns>
        public static bool Remove<T>(string key, bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            bool result = storage.Remove<T>(key);
            storage.TryDestroy(isSharedAcrossProjects);

            return result;
        }

        /// <summary>
        /// 移除指定类型的全部配置。
        /// </summary>
        /// <typeparam name="T">配置的类型。注意：此方法只会检查给定的类型，不会自动处理多态。</typeparam>
        /// <param name="isSharedAcrossProjects">配置是否在项目间共享。若为true，则在<see cref="UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder"/>/UserSettings文件夹中查找；否则在项目<see cref="Application.dataPath"/>/../UserSettings文件夹下查找。</param>
        /// <returns>是否成功移除了配置。</returns>
        public static bool RemoveAll<T>(bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            bool result = storage.RemoveAll<T>();
            storage.TryDestroy(isSharedAcrossProjects);

            return result;
        }

        /// <summary>
        /// 移除全部类型的全部配置。
        /// </summary>
        /// <param name="isSharedAcrossProjects">配置是否在项目间共享。若为true，则在<see cref="UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder"/>/UserSettings文件夹中查找；否则在项目<see cref="Application.dataPath"/>/../UserSettings文件夹下查找。</param>
        public static void Clear(bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            storage.Clear();
            storage.TryDestroy(isSharedAcrossProjects);
        }


        /// <summary>
        /// 开始批量编辑模式。
        /// 批量编辑模式下，修改配置时不会立即将配置写入文件，而是等到批量编辑模式结束时才写入。
        /// </summary>
        /// <param name="isSharedAcrossProjects">配置是否在项目间共享。若为true，则在<see cref="UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder"/>/UserSettings文件夹中查找；否则在项目<see cref="Application.dataPath"/>/../UserSettings文件夹下查找。</param>
        /// <returns>批处理区域对象。可配合using语句块使用。</returns>
        public static BatchingScope StartBatching(bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            BatchingScope scope = storage.StartBatching();
            storage.TryDestroy(isSharedAcrossProjects);

            return scope;
        }

        /// <summary>
        /// 结束批量编辑模式。
        /// </summary>
        /// <param name="isSharedAcrossProjects">配置是否在项目间共享。若为true，则在<see cref="UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder"/>/UserSettings文件夹中查找；否则在项目<see cref="Application.dataPath"/>/../UserSettings文件夹下查找。</param>
        public static void EndBatching(bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            storage.EndBatching();
            storage.TryDestroy(isSharedAcrossProjects);
        }
    }
}

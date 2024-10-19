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
            if (isSharedAcrossProjects && storage.BatchingCounter == 0)
            {
                storage.Destroy();
            }
        }


        public static bool Has<T>(string key, bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            bool has = storage.Has<T>(key);
            storage.TryDestroy(isSharedAcrossProjects);

            return has;
        }

        public static T Get<T>(string key, T defaultValue, bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            T value = storage.Get<T>(key, defaultValue);
            storage.TryDestroy(isSharedAcrossProjects);

            return value;
        }

        public static bool TryGet<T>(string key, out T value, bool isSharedAcrossProjects = false)
        {
            IEditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            bool result = storage.TryGet<T>(key, out value);
            storage.TryDestroy(isSharedAcrossProjects);

            return result;
        }

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
    }
}

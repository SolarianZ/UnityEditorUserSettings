namespace GBG.EditorUserSettings.Editor
{
    public static class EditorUserSettings
    {
        private static EditorUserSettingsStorage GetStorage(bool isSharedAcrossProjects)
        {
            return isSharedAcrossProjects
                ? EditorUserSettingsUserStorage.instance
                : EditorUserSettingsProjectStorage.instance;
        }

        private static void TryDestroy(this EditorUserSettingsStorage storage, bool isSharedAcrossProjects)
        {
            if (isSharedAcrossProjects && storage.BatchingCounter == 0)
            {
                storage.Destroy();
            }
        }


        public static bool Has<T>(string key, bool isSharedAcrossProjects = false)
        {
            EditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            bool has = storage.Has<T>(key);
            storage.TryDestroy(isSharedAcrossProjects);

            return has;
        }

        public static T Get<T>(string key, T defaultValue, bool isSharedAcrossProjects = false)
        {
            EditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            T value = storage.Get<T>(key, defaultValue);
            storage.TryDestroy(isSharedAcrossProjects);

            return value;
        }

        public static bool TryGet<T>(string key, out T value, bool isSharedAcrossProjects = false)
        {
            EditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            bool result = storage.TryGet<T>(key, out value);
            storage.TryDestroy(isSharedAcrossProjects);

            return result;
        }

        public static void Set<T>(string key, T value, bool isSharedAcrossProjects = false)
        {
            EditorUserSettingsStorage storage = GetStorage(isSharedAcrossProjects);
            storage.Set(key, value);
            storage.TryDestroy(isSharedAcrossProjects);
        }
    }
}

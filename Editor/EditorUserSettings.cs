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


        public static bool Has<T>(string key, bool isSharedAcrossProjects = false)
        {
            return GetStorage(isSharedAcrossProjects).Has<T>(key);
        }

        public static T Get<T>(string key, T defaultValue, bool isSharedAcrossProjects = false)
        {
            return GetStorage(isSharedAcrossProjects).Get<T>(key, defaultValue);
        }

        public static bool TryGet<T>(string key, out T value, bool isSharedAcrossProjects = false)
        {
            return GetStorage(isSharedAcrossProjects).TryGet<T>(key, out value);
        }

        public static void Set<T>(string key, T value, bool isSharedAcrossProjects = false)
        {
            GetStorage(isSharedAcrossProjects).Set(key, value);
        }
    }
}

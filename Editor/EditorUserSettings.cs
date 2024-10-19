namespace GBG.EditorUserSettings.Editor
{
    public static class EditorUserSettings
    {
        public static bool Has<T>(string key)
        {
            return EditorUserSettingsProjectStorage.instance.Has<T>(key);
        }

        public static T Get<T>(string key, T defaultValue)
        {
            return EditorUserSettingsProjectStorage.instance.Get<T>(key, defaultValue);
        }

        public static bool TryGet<T>(string key, out T value)
        {
            return EditorUserSettingsProjectStorage.instance.TryGet<T>(key, out value);
        }

        public static void Set<T>(string key, T value)
        {
            EditorUserSettingsProjectStorage.instance.Set(key, value);
        }
    }
}

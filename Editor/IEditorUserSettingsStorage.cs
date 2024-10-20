namespace GBG.EditorUserSettings.Editor
{
    internal interface IEditorUserSettingsStorage
    {
        uint BatchingCounter { get; }

        BatchingScope StartBatching();
        void EndBatching();


        bool Has<T>(string key);

        T Get<T>(string key, T defaultValue);
        bool TryGet<T>(string key, out T value);

        void Set<T>(string key, T value);

        bool Remove<T>(string key);
        bool RemoveAll<T>();
        void Clear();

        void Destroy();

        void ShowInFolder();
    }
}

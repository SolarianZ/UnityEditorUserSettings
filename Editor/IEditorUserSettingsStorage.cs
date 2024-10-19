namespace GBG.EditorUserSettings.Editor
{
    internal interface IEditorUserSettingsStorage
    {
        int BatchingCounter { get; }

        Batching StartBatching();
        void EndBatching();
    }
}

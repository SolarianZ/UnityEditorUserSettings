namespace GBG.EditorUserSettings.Editor
{
    internal interface IEditorUserSettingsStorage
    {
        uint BatchingCounter { get; }

        Batching StartBatching();
        void EndBatching();
    }
}

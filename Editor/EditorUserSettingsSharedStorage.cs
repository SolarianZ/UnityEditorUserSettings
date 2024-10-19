using UnityEditor;

namespace GBG.EditorUserSettings.Editor
{
    // Lock file? https://learn.microsoft.com/dotnet/api/system.io.filestream
    [FilePath(RelativePath, FilePathAttribute.Location.PreferencesFolder)]
    internal class EditorUserSettingsSharedStorage : EditorUserSettingsStorage<EditorUserSettingsSharedStorage>
    {
        public EditorUserSettingsSharedStorage()
        {
            TypeName = typeof(EditorUserSettingsSharedStorage).Name;
        }

        protected override void Save(bool saveAsText)
        {
            base.Save(saveAsText);

            if (BatchingCounter == 0)
            {
                Destroy();
            }
        }
    }

    [CustomEditor(typeof(EditorUserSettingsSharedStorage))]
    internal class EditorUserSettingsUserStorageInspector : UnityEditor.Editor
    {
        private void OnDisable()
        {
            ((EditorUserSettingsSharedStorage)target).Destroy();
        }
    }
}

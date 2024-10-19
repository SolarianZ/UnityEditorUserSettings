using UnityEditor;

namespace GBG.EditorUserSettings.Editor
{
    // Lock file? https://learn.microsoft.com/dotnet/api/system.io.filestream
    [FilePath(RelativePath, FilePathAttribute.Location.PreferencesFolder)]
    internal class EditorUserSettingsUserStorage : EditorUserSettingsStorage
    {
        protected override void Save(bool saveAsText)
        {
            base.Save(saveAsText);

            if (BatchingCounter == 0)
            {
                Destroy();
            }
        }
    }
}

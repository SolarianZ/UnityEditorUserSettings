using UnityEditor;

namespace GBG.EditorUserSettings.Editor
{
    // Lock file?
    [FilePath(RelativePath, FilePathAttribute.Location.PreferencesFolder)]
    internal class EditorUserSettingsUserStorage : EditorUserSettingsStorage { }
}

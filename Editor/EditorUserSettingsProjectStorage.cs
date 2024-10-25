using UnityEditor;

namespace GBG.EditorUserSettings.Editor
{
    [FilePath(RelativePath, FilePathAttribute.Location.ProjectFolder)]
    internal class EditorUserSettingsProjectStorage : EditorUserSettingsStorage<EditorUserSettingsProjectStorage>
    {
#if GBG_EDITOR_USER_SETTINGS_USE_LIBRARY
        public new const string RelativePath = "Library/" + EditorUserSettingsStorage<EditorUserSettingsProjectStorage>.RelativePath;
#endif
    }
}

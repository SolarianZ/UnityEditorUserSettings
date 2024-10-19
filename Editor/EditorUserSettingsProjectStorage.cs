using UnityEditor;

namespace GBG.EditorUserSettings.Editor
{
    [FilePath(RelativePath, FilePathAttribute.Location.ProjectFolder)]
    internal class EditorUserSettingsProjectStorage : EditorUserSettingsStorage<EditorUserSettingsProjectStorage>
    {
        public EditorUserSettingsProjectStorage()
        {
            TypeName = typeof(EditorUserSettingsProjectStorage).Name;
        }
    }
}

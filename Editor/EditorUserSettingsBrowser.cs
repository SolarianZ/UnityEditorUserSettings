using System;
using UnityEditor;
using UnityEngine;

namespace GBG.EditorUserSettings.Editor
{
    [Serializable]
    public class MyClassSerializable
    {
        public string name = "sssss";
        public int value = 123;
    }

    public class MyClassNonSerializable
    {
        public string name = "nnnn";
        public int value = 34534;
    }

    public class EditorUserSettingsBrowser : EditorWindow, IHasCustomMenu
    {
        [MenuItem("Tools/Bamboo/Editor User Settings Browser")]
        public static void Open()
        {
            GetWindow<EditorUserSettingsBrowser>("Editor User Settings Browser").Focus();
        }

        private void OnEnable()
        {
            Debug.Log(EditorUserSettings.Get("key_string", "ERROR"));
            Debug.Log(EditorUserSettings.Get("key_int", int.MinValue));
            Debug.Log(EditorUserSettings.Get("key_bool", false));
            Debug.Log(EditorUserSettings.Get("key_vector3", Vector3.zero));
            Debug.Log(EditorUserSettings.Get("key_system.vector3", System.Numerics.Vector3.Zero));
            Debug.Log(EditorUserSettings.Get<MyClassSerializable>("key_MyClassSerializable", null));
            Debug.Log(EditorUserSettings.Get<MyClassNonSerializable>("key_MyClassNonSerializable", null));
        }

        private void OnDisable()
        {
            EditorUserSettings.Set("key_string", DateTime.Now.ToString());
            EditorUserSettings.Set("key_int", 123);
            EditorUserSettings.Set("key_bool", true);
            EditorUserSettings.Set("key_vector3", Vector3.one);
            EditorUserSettings.Set("key_system.vector3", System.Numerics.Vector3.One);
            EditorUserSettings.Set("key_MyClassSerializable", new MyClassSerializable());
            EditorUserSettings.Set("key_MyClassNonSerializable", new MyClassNonSerializable());
        }

        #region IHasCustomMenu

        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Inspect Project Storage Object"), false,
                () => { Selection.activeObject = EditorUserSettingsProjectStorage.instance; });

            menu.ShowAsContext();
        }

        #endregion
    }
}

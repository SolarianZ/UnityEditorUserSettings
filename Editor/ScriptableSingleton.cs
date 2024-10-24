﻿#if !UNITY_2020_1_OR_NEWER
using System;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace GBG.EditorUserSettings.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class FilePathAttribute : Attribute
    {
        public enum Location { PreferencesFolder, ProjectFolder }

        private string filePath;
        private string relativePath;
        private Location location;

        public string filepath
        {
            get
            {
                if (filePath == null && relativePath != null)
                {
                    filePath = GetFilePath(relativePath, location);
                    relativePath = null;
                }

                return filePath;
            }

            set
            {
                filePath = value;
            }
        }

        public FilePathAttribute(string relativePath, Location location)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                Debug.LogError("Invalid relative path! (its null or empty)");
                return;
            }

            this.relativePath = relativePath;
            this.location = location;
        }

        static string GetFilePath(string relativePath, Location location)
        {
            // We do not want a slash as first char
            if (relativePath[0] == '/')
                relativePath = relativePath.Substring(1);

            if (location == Location.PreferencesFolder)
                return InternalEditorUtility.unityPreferencesFolder + "/" + relativePath;
            else //location == Location.ProjectFolder
                return relativePath;
        }
    }

    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        static T s_Instance;

        public static T instance
        {
            get
            {
                if (s_Instance == null)
                    CreateAndLoad();

                return s_Instance;
            }
        }

        // On domain reload ScriptableObject objects gets reconstructed from a backup. We therefore set the s_Instance here
        protected ScriptableSingleton()
        {
            if (s_Instance != null)
            {
                Debug.LogError("ScriptableSingleton already exists. Did you query the singleton in a constructor?");
            }
            else
            {
                object casted = this;
                s_Instance = casted as T;
                System.Diagnostics.Debug.Assert(s_Instance != null);
            }
        }

        private static void CreateAndLoad()
        {
            System.Diagnostics.Debug.Assert(s_Instance == null);

            // Load
            string filePath = GetFilePath();
            if (!string.IsNullOrEmpty(filePath))
            {
                // If a file exists the
                InternalEditorUtility.LoadSerializedFileAndForget(filePath);
            }

            if (s_Instance == null)
            {
                // Create
                T t = CreateInstance<T>();
                t.hideFlags = HideFlags.HideAndDontSave;
            }

            System.Diagnostics.Debug.Assert(s_Instance != null);
        }

        protected virtual void Save(bool saveAsText)
        {
            if (s_Instance == null)
            {
                Debug.Log("Cannot save ScriptableSingleton: no instance!");
                return;
            }

            string filePath = GetFilePath();
            if (!string.IsNullOrEmpty(filePath))
            {
                string folderPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                InternalEditorUtility.SaveToSerializedFileAndForget(new[] { s_Instance }, filePath, saveAsText);
            }
        }

        protected static string GetFilePath()
        {
            Type type = typeof(T);
            object[] atributes = type.GetCustomAttributes(true);
            foreach (object attr in atributes)
            {
                if (attr is FilePathAttribute)
                {
                    FilePathAttribute f = attr as FilePathAttribute;
                    return f.filepath;
                }
            }
            return null;
        }
    }
}
#endif
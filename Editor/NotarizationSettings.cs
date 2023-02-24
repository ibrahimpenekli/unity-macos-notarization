using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Inscept.Notarization
{
    [Flags]
    public enum EntitlementsOptions
    {
        None = 0,
        MonoScriptingBackend = 1,
        Steamworks = 2
    }
    
    public class NotarizationSettings : ScriptableObject
    {
        private const string SettingsPath = "Assets/Editor/NotarizationSettings.asset";

        [SerializeField]
        [Tooltip("Enable OSX notarization on Build.")]
        private bool _enableNotarization;
        
        public bool enableNotarization
        {
            get => _enableNotarization;
            set => _enableNotarization = value;
        }
        
        [SerializeField]
        [Tooltip("Developer ID Application : XXX (YYY)")]
        private string _developerCertificateId;

        public string developerCertificateId
        {
            get => _developerCertificateId;
            set => _developerCertificateId = value;
        }

        [SerializeField]
        [Tooltip("Apple ID")]
        private string _appleId;

        public string appleId
        {
            get => _appleId;
            set => _appleId = value;
        }

        [SerializeField]
        [Tooltip("xxxx-xxxx-xxxx-xxxx")]
        private string _appPassword;

        public string appPassword
        {
            get => _appPassword;
            set => _appPassword = value;
        }

        [SerializeField]
        [Tooltip("Apple Team ID")]
        private string _teamId;

        public string teamId
        {
            get => _teamId;
            set => _teamId = value;
        }
        
        [SerializeField]
        [Tooltip("Custom entitlements file. Optional.")]
        private TextAsset _customEntitlementsFile;

        public TextAsset customEntitlementsFile
        {
            get => _customEntitlementsFile;
            set => _customEntitlementsFile = value;
        }

        internal static NotarizationSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<NotarizationSettings>(SettingsPath);
            if (settings == null)
            {
                var settingsPath = SettingsPath.Replace("Assets/", "");
                var settingsDirectory = Path.GetDirectoryName(Path.Combine(Application.dataPath, settingsPath)) ?? "";

                if (!Directory.Exists(settingsDirectory))
                {
                    Directory.CreateDirectory(settingsDirectory);
                }

                settings = CreateInstance<NotarizationSettings>();

                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
        
        internal static bool IsSettingsAvailable()
        {
            var settings = AssetDatabase.LoadAssetAtPath<NotarizationSettings>(SettingsPath);
            return settings != null;
        }
    }
}
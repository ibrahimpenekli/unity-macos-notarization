using UnityEditor;
using UnityEngine.UIElements;

namespace Inscept.OSX.Notarization
{
    public class NotarizationSettingsProvider : SettingsProvider
    {
        private const string SettingsSectionPath = "Project/OSX Notarization Settings";

        private SerializedObject _settings;

        public NotarizationSettingsProvider(string path, SettingsScope scope = SettingsScope.User)
            : base(path, scope)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = NotarizationSettings.GetSerializedSettings();
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_settings.FindProperty("_enableNotarization"));
            EditorGUILayout.PropertyField(_settings.FindProperty("_developerCertificateId"));
            EditorGUILayout.PropertyField(_settings.FindProperty("_appleId"));
            EditorGUILayout.PropertyField(_settings.FindProperty("_appPassword"));
            EditorGUILayout.PropertyField(_settings.FindProperty("_teamId"));
            EditorGUILayout.PropertyField(_settings.FindProperty("_customEntitlementsFile"));

            _settings.ApplyModifiedPropertiesWithoutUndo();
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            NotarizationSettings.GetOrCreateSettings();

            var provider = new NotarizationSettingsProvider(SettingsSectionPath, SettingsScope.Project);
            provider.keywords = new[] { "notarize", "notarization", "codesign", "code sign" };
            return provider;
        }
    }
}
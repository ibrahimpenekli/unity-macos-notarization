using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Inscept.OSX.Notarization
{
    public class NotaryPostprocessBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder => 9999;

        public void OnPostprocessBuild(BuildReport report)
        {
#if !UNITY_CLOUD_BUILD
            if (report.summary.platform == BuildTarget.StandaloneOSX && !IsDevelopmentBuild(report))
            {
                if (NotarizationSettings.IsSettingsAvailable())
                {
                    var settings = NotarizationSettings.GetOrCreateSettings();
                    if (settings.enableNotarization)
                    {
                        Notarization.Submit(report, settings);
                    }
                }
            }
#endif
        }

        private static bool IsDevelopmentBuild(BuildReport report)
        {
            return (report.summary.options & BuildOptions.Development) != 0;
        }
    }
}
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Inscept.Notarization
{
    public class NotarizationUtility
    {
        
        /// <summary>
        /// Submits built app to the Apple Notary Service for notarization. See <see cref="Notary.Submit"/>.
        /// </summary>
        /// <param name="report">The build report.</param>
        /// <param name="settings">The notarization settings. Default notarization settings will be used if it is left empty.</param>
        /// <param name="entitlementsOptions">The entitlements file creation options.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="report"/> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">If build target is not OSX platform.</exception>
        public static void Submit(BuildReport report, NotarizationSettings settings = null, 
            EntitlementsOptions? entitlementsOptions = null)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            if (report.summary.platform != BuildTarget.StandaloneOSX)
                throw new NotSupportedException(
                    $"App notarization is supported on {BuildTarget.StandaloneOSX} platform only.");

            if (settings == null)
            {
                settings = NotarizationSettings.GetOrCreateSettings();
            }

            var entitlementsPath = "";

            try
            {
                if (!entitlementsOptions.HasValue)
                {
                    entitlementsOptions = EntitlementsOptions.None;
                    
                    if (PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup) ==
                        ScriptingImplementation.Mono2x)
                    {
                        entitlementsOptions |= EntitlementsOptions.MonoScriptingBackend;
                    }
                    
#if STEAMWORKS
                    entitlementsOptions |= EntitlementsOptions.Steamworks;
#endif
                }
                
                
                entitlementsPath = settings.customEntitlementsFile != null
                    ? CreateEntitlementsFile(report.summary.outputPath, settings.customEntitlementsFile)
                    : CreateEntitlementsFile(report.summary.outputPath, entitlementsOptions.Value);

                var notary =
                    new Notary(
                        settings.developerCertificateId,
                        settings.appleId,
                        settings.appPassword,
                        settings.teamId,
                        entitlementsPath);

                notary.Submit(report.summary.outputPath);
            }
            finally
            {
                if (File.Exists(entitlementsPath))
                {
                    File.Delete(entitlementsPath);
                }
            }
        }

        /// <summary>
        /// Creates entitlements file needed for code signing process.
        /// </summary>
        /// <param name="outputPath">The output path for the build.</param>
        /// <param name="options">The entitlements file creation options.</param>
        /// <returns>The entitlements file path.</returns>
        public static string CreateEntitlementsFile(string outputPath, EntitlementsOptions options)
        {
            var entitlementsPath = GetEntitlementsFilePath(outputPath);

            var entitlements = new StringBuilder();
            entitlements.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            entitlements.AppendLine(
                "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">");
            entitlements.AppendLine("<plist version=\"1.0\">");
            entitlements.AppendLine("<dict>");
            entitlements.AppendLine("<key>com.apple.security.cs.disable-library-validation</key><true/>");
            entitlements.AppendLine("<key>com.apple.security.cs.disable-executable-page-protection</key><true/>");

            // Needed for Mono scripting backend.
            if (options.HasFlag(EntitlementsOptions.MonoScriptingBackend))
            {
                entitlements.AppendLine("<key>com.apple.security.cs.allow-unsigned-executable-memory</key><true/>");
            }

            if (options.HasFlag(EntitlementsOptions.Steamworks))
            {
                // Needed for Steam Overlay.
                entitlements.AppendLine("<key>com.apple.security.cs.allow-dyld-environment-variables</key><true/>");    
            }
            
            entitlements.AppendLine("</dict>");
            entitlements.AppendLine("</plist>");

            File.WriteAllText(entitlementsPath, entitlements.ToString());
            return entitlementsPath;
        }

        private static string GetEntitlementsFilePath(string outputPath)
        {
            var outputDirectory = Path.GetDirectoryName(outputPath) ?? "";
            return Path.Combine(outputDirectory, $"{Application.productName}.entitlements");
        }

        private static string CreateEntitlementsFile(string outputPath, TextAsset entitlements)
        {
            var entitlementsPath = GetEntitlementsFilePath(outputPath);
            File.WriteAllText(entitlementsPath, entitlements.text);
            return entitlementsPath;
        }
    }
}
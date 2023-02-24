using System;
using Inscept.Notarization.Shell;
using UnityEngine;

namespace Inscept.Notarization
{
    public class Notary
    {
        private string developerCertificateId { get; }
        private string appleId { get; }
        private string appPassword { get; }
        private string teamId { get; }
        private string entitlementsPath { get; }

        public Notary(string developerCertificateId, string appleId, string appPassword, string teamId,
            string entitlementsPath)
        {
            this.developerCertificateId = developerCertificateId;
            this.appleId = appleId;
            this.appPassword = appPassword;
            this.teamId = teamId;
            this.entitlementsPath = entitlementsPath;
        }

        public void Submit(string buildOutputPath)
        {
            var scripts = new ShellScript[]
            {
                new CodeSignScript(entitlementsPath, developerCertificateId, buildOutputPath),
                new NotarizeScript(appleId, appPassword, teamId, buildOutputPath),
                new StapleScript(buildOutputPath)
            };

            foreach (var script in scripts)
            {
                var exitCode = script.Run();

                if (script.standardOutput != null)
                {
                    Debug.Log(script.standardOutput);
                }
                
                if (script.standardError != null)
                {
                    Debug.Log(script.standardError);
                }
                
                if (exitCode != 0)
                    throw new Exception($"Script failed: {script.GetType().Name}, exit code: {exitCode}");
            }
        }
    }
}
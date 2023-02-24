using System;
using UnityEngine.Windows;

namespace Inscept.Notarization.Shell
{
    public class NotarizeScript : ShellScript
    {
        private readonly string _buildOutputPath;
        
        public NotarizeScript(string appleId, string appleAppPassword, string appleTeamId, string buildOutputPath)
            : base("notarize.sh")
        {
            _buildOutputPath = buildOutputPath;
            SetArguments(appleId, appleAppPassword, appleTeamId, buildOutputPath);
        }

        protected override int RunInternal(bool blockingWait)
        {
            var exitCode = base.RunInternal(blockingWait);
            
            if (blockingWait)
            {
                // Cleanup.
                try
                {
                    var archiveFile = $"{_buildOutputPath}.zip";
                    if (File.Exists(archiveFile))
                    {
                        File.Delete(archiveFile);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return exitCode;
        }
    }
}
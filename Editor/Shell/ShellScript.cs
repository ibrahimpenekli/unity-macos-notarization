using System.Diagnostics;

namespace Inscept.Notarization.Shell
{
    public abstract class ShellScript
    {
        private readonly string _scriptPath;
        private string _args;
        
        public string standardOutput { get; private set; }
        public string standardError { get; private set; }

        protected ShellScript(string scriptFile)
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(Notary).Assembly);
            _scriptPath = $"{packageInfo.resolvedPath}/Editor/Shell/Scripts/{scriptFile}";
        }
        
        protected void SetArguments(params string[] args)
        {
            _args = GetEscapedArguments(args);
        }

        public int Run()
        {
            return RunInternal(true);
        }

        public void RunNonBlocking()
        {
            RunInternal(false);
        }

        protected virtual int RunInternal(bool blockingWait)
        {
            using var process = new Process();
            process.StartInfo.FileName = "/bin/sh";
            process.StartInfo.Arguments = $"{_scriptPath} {_args}";
            
            // Debug.Log($"{process.StartInfo.FileName} {process.StartInfo.Arguments}");

            if (blockingWait)
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                // Synchronously read the standard output of the spawned process.
                var reader = process.StandardOutput;
                var errorReader = process.StandardError;
                standardOutput = reader.ReadToEnd();
                standardError = errorReader.ReadToEnd();

                process.WaitForExit();
                return process.ExitCode;
            }

            process.Start();
            return 0;
        }
        
        /// <summary>
        /// Quotes all arguments that contain whitespace, or begin with a quote and returns a single
        /// argument string for use with Process.Start().
        /// </summary>
        private static string GetEscapedArguments(params string[] args)
        {
            return string.Concat(" \"", string.Join("\" \"", args), "\"");
        }
    }
}
namespace Inscept.Notarization.Shell
{
    public class CodeSignScript : ShellScript
    {
        public CodeSignScript(string entitlementsPath, string developerCertId, string buildOutputPath) 
            : base("codesign.sh")
        {
            SetArguments(entitlementsPath, developerCertId, buildOutputPath);
        }
    }
}
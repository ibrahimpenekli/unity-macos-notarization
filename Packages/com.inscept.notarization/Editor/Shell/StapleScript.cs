namespace Inscept.Notarization.Shell
{
    public class StapleScript : ShellScript
    {
        public StapleScript(string buildOutputPath) : base("staple.sh")
        {
            SetArguments(buildOutputPath);
        }
    }
}
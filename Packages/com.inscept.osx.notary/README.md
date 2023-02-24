# OSX Notarization for Unity
Notarization is a process where Apple verifies your application to make sure it has a Developer ID code signature and doesn’t contain malicious content. 
For more information about notarization, see Apple’s documentation on [Notarizing macOS Software Before Distribution](https://developer.apple.com/documentation/security/notarizing_macos_software_before_distribution).

## Install via Unity Package Manager:
* Add `"com.inscept.osx.notary": "https://github.com/ibrahimpenekli/unity-macos-notary.git#1.0.0"` to your project's package manifest file in dependencies section.
* Or, `Package Manager > Add package from git URL...` and paste this URL: `https://github.com/ibrahimpenekli/unity-macos-notary.git#1.0.0`

## How to Use?

Either you can enable notarization for non-development builds, or you can directly call notarization process within your build pipeline as follows:

```csharp
public class MyPostprocessBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder => 9999;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.StandaloneOSX)
            {
                Notarization.Submit(outputPath);
            }
        }
    }
```
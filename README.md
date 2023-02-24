# OSX Notarization for Unity
Notarization is a process where Apple verifies your application to make sure it has a Developer ID code signature and doesn’t contain malicious content. 
For more information about notarization, see Apple’s documentation on [Notarizing macOS Software Before Distribution](https://developer.apple.com/documentation/security/notarizing_macos_software_before_distribution).

## Install via Unity Package Manager:
* Add `"com.inscept.notarization": "https://github.com/ibrahimpenekli/unity-macos-notarization.git#1.0.0"` to your project's package manifest file in dependencies section.
* Or, `Package Manager > Add package from git URL...` and paste this URL: `https://github.com/ibrahimpenekli/unity-macos-notarization.git#1.0.0`

## How to Use?

You can notarize your app either using in post process build automatically or you can manually call notarization method in your custom build pipeline.

### Automatic Usage

Go to `Project Settings > OSX Notarization` and set your information. Don't forget to enable notarization in the settings. You're done!
Notarization process is done in post process build script automatically. 

Notarization process will be ignored for these cases:
* Build target is different than `StandaloneOSX`
* Development build
* Build is completed by Unity Cloud Build 

### Manual Usage

You can directly call notarization process within your build pipeline as follows:

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

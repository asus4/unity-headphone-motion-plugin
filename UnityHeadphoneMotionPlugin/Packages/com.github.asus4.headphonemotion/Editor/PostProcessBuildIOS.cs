#if UNITY_IOS

using System.IO;
using UnityEngine;
using UnityEditor;
// using UnityEditor.Callbacks;
// using UnityEditor.iOS.Xcode;

namespace HeadphoneMotion.Editor
{
    // public static class PostProcessBuildIOS
    // {
    //     [PostProcessBuild]
    //     public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    //     {
    //         if (buildTarget == BuildTarget.iOS)
    //         {
    //             UnityEngine.Debug.Log("OnPostprocessBuild");
    //             EditInfoPlist(Path.Combine(path, "Info.plist"));
    //         }
    //     }


    //     private static void EditInfoPlist(string plistPath)
    //     {
    //         PlistDocument plist = new PlistDocument();
    //         plist.ReadFromString(File.ReadAllText(plistPath));

    //         // Get root
    //         PlistElementDict rootDict = plist.root;
    //         rootDict.SetString("NSMotionUsageDescription", "For spacial audio");

    //         // Save updated
    //         File.WriteAllText(plistPath, plist.WriteToString());
    //     }

    // }
}

#endif
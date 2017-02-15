#if UNITY_IOS
using UnityEngine; 
using UnityEditor; 
using UnityEditor.Callbacks; 
using UnityEditor.iOS.Xcode; 
using System.Collections; 
using System.Collections.Generic; 
using System.Diagnostics; 
using System.IO; 
using System.Linq; 

namespace UnitySwift {
	public static class PostProcessor {
		//gamecenterinunity.entitlements 請改成[自己專案的名稱].entitlements
		const string ENTITLEMENTS_NAME = "gamecenterinunity.entitlements"; 
		//這串數字請換成自己的
		const string ENTITLEMENTS_REFERENCE_CODE = "1234567890ABCDEF12345678"; 

		[PostProcessBuild]
		public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath) {
			if (buildTarget == BuildTarget.iOS) {
				
				var projPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj"; 
				var proj = new PBXProject(); 
				proj.ReadFromFile(projPath); 

				var targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName()); 

				// Configure build settings
				proj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO"); 
				proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/UnitySwift/UnitySwift-Bridging-Header.h"); 
				proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_INTERFACE_HEADER_NAME", "unityswift-Swift.h"); 
				proj.AddBuildProperty(targetGuid, "SWIFT_VERSION", "3.0"); 
				proj.AddBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks"); 
				proj.AddBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", string.Format("Unity-iPhone/{0}", ENTITLEMENTS_NAME)); 

				proj.WriteToFile(projPath); 

				AddDeviceCapabilities(buildPath); 
				AddEntitlements(buildPath); 
				EnableiCloud(proj, projPath); 
			}
		}

		static void AddDeviceCapabilities(string pathToBuiltProject) {
			string infoPlistPath = Path.Combine(pathToBuiltProject, "./Info.plist"); 
			PlistDocument plist = new PlistDocument(); 
			plist.ReadFromString(File.ReadAllText(infoPlistPath)); 

			PlistElementDict rootDict = plist.root; 
			PlistElementArray deviceCapabilityArray = rootDict.CreateArray("UIRequiredDeviceCapabilities"); 
			deviceCapabilityArray.AddString("armv7"); 
			deviceCapabilityArray.AddString("gamekit"); 

			File.WriteAllText(infoPlistPath, plist.WriteToString()); 
		}

		static void AddEntitlements(string pathToBuiltProject) {
			string entitlementsPath = Path.Combine(pathToBuiltProject, string.Format("./Unity-iPhone/{0}", ENTITLEMENTS_NAME)); 
			PlistDocument plist = new PlistDocument(); 
			PlistElementDict rootDict = plist.root; 
			PlistElementArray deviceCapabilityArray = rootDict.CreateArray("com.apple.developer.icloud-container-identifiers"); 
			deviceCapabilityArray.AddString("iCloud.$(CFBundleIdentifier)"); 
			PlistElementArray serviceArray = rootDict.CreateArray("com.apple.developer.icloud-services"); 
			serviceArray.AddString("CloudDocuments"); 
			PlistElementArray ubiquityArray = rootDict.CreateArray("com.apple.developer.ubiquity-container-identifiers"); 
			ubiquityArray.AddString("iCloud.$(CFBundleIdentifier)"); 
			rootDict.SetString("com.apple.developer.ubiquity-kvstore-identifier", "$(TeamIdentifierPrefix)$(CFBundleIdentifier)"); 

			using(StreamWriter sw = File.CreateText(entitlementsPath)) {
				sw.WriteLine(plist.WriteToString()); 
			}
		}

		static void EnableiCloud(PBXProject project, string projectPath) {
			string projectString = project.WriteToString(); 
			
			//add entitlements file
			projectString = projectString.Replace("/* Begin PBXFileReference section */", "/* Begin PBXFileReference section */\n\t\t" + ENTITLEMENTS_REFERENCE_CODE + " /* " + ENTITLEMENTS_NAME + " */ = {isa = PBXFileReference; lastKnownFileType = text.xml; name = " + ENTITLEMENTS_NAME + "; path = \"Unity - iPhone/" + ENTITLEMENTS_NAME + "\"; sourceTree = \" < group > \"; };"); 

			//add entitlements file (again)
			projectString = projectString.Replace("/* CustomTemplate */ = {\n			isa = PBXGroup;\n			children = (", "/* CustomTemplate */ = {\n			isa = PBXGroup;\n			children = (\n				" + ENTITLEMENTS_REFERENCE_CODE + " /* " + ENTITLEMENTS_NAME + " */,"); 

			//save the file
			File.WriteAllText(projectPath, projectString); 
		}

	}
}
#endif
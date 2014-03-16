using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Uzu
{
	static public class BuildMenu
	{
		private const string UZU_DBG = "UZU_DBG";

		private const string UZU_GAMESTICK = "UZU_GAMESTICK";
		private const string UZU_GOOGLEPLAY = "UZU_GOOGLEPLAY";
		private const string UZU_OUYA = "UZU_OUYA";

		/// <summary>
		/// An array of all the possible platform defines.
		/// </summary>
		private static readonly string[] PLATFORM_DEFINES = {
			UZU_GAMESTICK,
			UZU_GOOGLEPLAY,
			UZU_OUYA,
		};

		[MenuItem("Uzu/Build Settings/Config Debug", false, 1)]
		private static void ConfigDebug (MenuCommand command)
		{
			AddDefineForPlatform (UZU_DBG, ActiveBuildTargetToBuildTargetGroup ());
		}

		[MenuItem("Uzu/Build Settings/Config Release", false, 1)]
		private static void ConfigRelease (MenuCommand command)
		{
			RemoveDefineForPlatform (UZU_DBG, ActiveBuildTargetToBuildTargetGroup ());
		}

		[MenuItem("Uzu/Build Settings/", false, 2)]
		static void Breaker () { }

		#region Platforms.
		[MenuItem("Uzu/Build Settings/Config GameStick", false, 3)]
		private static void ConfigGameStick (MenuCommand command)
		{
			BuildTargetGroup targetGroup = BuildTargetGroup.Android;

			foreach (string platform in PLATFORM_DEFINES) {
				RemoveDefineForPlatform (platform, targetGroup);
			}

			AddDefineForPlatform (UZU_GAMESTICK, targetGroup);
		}

		[MenuItem("Uzu/Build Settings/Config GameStick", true, 3)]
		private static bool ConfigGameStickCheck ()
		{
			return EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
		}

		[MenuItem("Uzu/Build Settings/Config Google Play", false, 3)]
		private static void ConfigGooglePlay (MenuCommand command)
		{
			BuildTargetGroup targetGroup = BuildTargetGroup.Android;
			
			foreach (string platform in PLATFORM_DEFINES) {
				RemoveDefineForPlatform (platform, targetGroup);
			}
			
			AddDefineForPlatform (UZU_GOOGLEPLAY, targetGroup);
		}
		
		[MenuItem("Uzu/Build Settings/Config Google Play", true, 3)]
		private static bool ConfigGooglePlayCheck ()
		{
			return EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
		}

		[MenuItem("Uzu/Build Settings/Config OUYA", false, 3)]
		private static void ConfigOUYA (MenuCommand command)
		{
			BuildTargetGroup targetGroup = BuildTargetGroup.Android;
			
			foreach (string platform in PLATFORM_DEFINES) {
				RemoveDefineForPlatform (platform, targetGroup);
			}
			
			AddDefineForPlatform (UZU_OUYA, targetGroup);
		}
		
		[MenuItem("Uzu/Build Settings/Config OUYA", true, 3)]
		private static bool ConfigOUYACheck ()
		{
			return EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
		}
		#endregion

		private static void AddDefineForPlatform (string define, BuildTargetGroup targetGroup)
		{
			if (targetGroup == BuildTargetGroup.Unknown) {
				return;
			}

			// Remove in case it already exists.
			RemoveDefineForPlatform (define, targetGroup);

			string currentDefineStr = PlayerSettings.GetScriptingDefineSymbolsForGroup (targetGroup);
			string newDefineStr = currentDefineStr + ";" + define;
			PlayerSettings.SetScriptingDefineSymbolsForGroup (targetGroup, newDefineStr);
		}

		private static void RemoveDefineForPlatform (string define, BuildTargetGroup targetGroup)
		{
			if (targetGroup == BuildTargetGroup.Unknown) {
				return;
			}

			string currentDefineStr = PlayerSettings.GetScriptingDefineSymbolsForGroup (targetGroup);
			List<string> currentDefines = new List<string> (currentDefineStr.Split (new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
			currentDefines.RemoveAll (str => (str == define));
			string newDefineStr = string.Join (";", currentDefines.ToArray ());
			PlayerSettings.SetScriptingDefineSymbolsForGroup (targetGroup, newDefineStr);
		}

		private static BuildTargetGroup ActiveBuildTargetToBuildTargetGroup ()
		{
			BuildTarget bt = EditorUserBuildSettings.activeBuildTarget;
			if (bt == BuildTarget.Android) {
				return BuildTargetGroup.Android;
			}
			else if (bt == BuildTarget.iPhone) {
				return BuildTargetGroup.iPhone;
			}
			else if (bt == BuildTarget.WebPlayer) {
				return BuildTargetGroup.WebPlayer;
			}
			else if (bt == BuildTarget.WP8Player) {
				return BuildTargetGroup.WP8;
			}
			else if (bt == BuildTarget.StandaloneOSXIntel ||
			         bt == BuildTarget.StandaloneOSXIntel64 ||
			         bt == BuildTarget.StandaloneOSXUniversal ||
			         bt == BuildTarget.StandaloneWindows ||
			         bt == BuildTarget.StandaloneWindows64 ||
			         bt == BuildTarget.StandaloneLinux ||
			         bt == BuildTarget.StandaloneLinux64 ||
			         bt == BuildTarget.StandaloneLinuxUniversal) {
				return BuildTargetGroup.Standalone;
			}

			Debug.LogWarning ("Unhandled build target: " + bt);
			return BuildTargetGroup.Unknown;
		}
	}
}
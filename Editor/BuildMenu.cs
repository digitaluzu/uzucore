using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Uzu
{
	static public class BuildMenu
	{
		private const string UZU_DBG = "UZU_DBG";

		private static readonly BuildTargetGroup[] TARGET_GROUPS = {
			BuildTargetGroup.Android,
			BuildTargetGroup.iPhone,
			BuildTargetGroup.WebPlayer,
		};

		[MenuItem("Uzu/Build Settings/Config Debug")]
		private static void ConfigDebug (MenuCommand command)
		{
			foreach (BuildTargetGroup targetGroup in TARGET_GROUPS) {
				AddDefineForPlatform (UZU_DBG, targetGroup);
			}
		}

		[MenuItem("Uzu/Build Settings/Config Release")]
		private static void ConfigRelease (MenuCommand command)
		{
			foreach (BuildTargetGroup targetGroup in TARGET_GROUPS) {
				RemoveDefineForPlatform (UZU_DBG, targetGroup);
			}
		}

		private static void AddDefineForPlatform (string define, BuildTargetGroup targetGroup)
		{
			// Remove in case it already exists.
			RemoveDefineForPlatform (define, targetGroup);

			string currentDefineStr = PlayerSettings.GetScriptingDefineSymbolsForGroup (targetGroup);
			string newDefineStr = currentDefineStr + ";" + define;
			PlayerSettings.SetScriptingDefineSymbolsForGroup (targetGroup, newDefineStr);
		}

		private static void RemoveDefineForPlatform (string define, BuildTargetGroup targetGroup)
		{
			string currentDefineStr = PlayerSettings.GetScriptingDefineSymbolsForGroup (targetGroup);
			List<string> currentDefines = new List<string> (currentDefineStr.Split (new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
			currentDefines.RemoveAll (str => (str == define));
			string newDefineStr = string.Join (";", currentDefines.ToArray ());
			PlayerSettings.SetScriptingDefineSymbolsForGroup (targetGroup, newDefineStr);
		}
	}
}
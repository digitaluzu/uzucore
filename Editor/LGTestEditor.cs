using UnityEditor;
using UnityEngine;

public class LGTestEditor : EditorWindow
{
	// Add menu item named "My Window" to the Window menu
	[MenuItem("Lunar/Test Unit")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(LGTestEditor));
	}

	void OnGUI()
	{
		GUILayout.Label ("Planing on making a TEST UNIT HERE but ...How?", EditorStyles.boldLabel);
	}
}
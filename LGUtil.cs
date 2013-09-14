using UnityEngine;

/// <summary>
/// Miscellaneous services.
/// </summary>
public class LGUtil
{
	/// <summary>
	/// Sets the "instance" variable of a singleton object to a new value.
	/// Performs error checking to make sure the instance is not already set.
	/// </summary>
	public static void SingletonSet<T> (ref T instance, T newValue)
	{
		if (instance != null && newValue != null) {
			Debug.LogError ("Singleton instance is already set! [" + typeof(T).ToString () + "].");
			return;
		}
		instance = newValue;
	}

	/// <summary>
	/// Attempts to change the current scene.
	/// </summary>
	public static void RequestSceneChange (string sceneName)
	{
		// This GO should be released automatically when scene change is completed.
//		GameObject go = new GameObject ("SceneChangerGO");
//		LGSceneChanger sceneChanger = go.AddComponent<LGSceneChanger> ();
//		sceneChanger.RequestSceneChange (sceneName);
	}
	
	public class Device {
		//@todo: allow access to all device settings through interface. LGDevice? LGUtil.Device?
		public static bool IsHighResolution()
		{
			return Screen.height > TargetScreenHeight;
		}
		
		public static int TargetScreenWidth
		{
			get { return 320; }
//			get { return 480; }
		}
		
		public static int TargetScreenHeight
		{
			get { return 480; }
//			get { return 320; }
		}
	}
}

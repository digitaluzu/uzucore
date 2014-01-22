using UnityEngine;

namespace Uzu
{
	/// <summary>
	/// Miscellaneous services.
	/// </summary>
	public static class Util
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
		/// Finds the specified component on the game object or one of its parents.
		/// </summary>
		public static T FindInParents<T> (GameObject go) where T : Component
		{
			if (go == null) {
				return null;
			}

			object comp = go.GetComponent<T>();
			
			if (comp == null) {
				Transform t = go.transform.parent;
				
				while (t != null && comp == null) {
					comp = t.gameObject.GetComponent<T>();
					t = t.parent;
				}
			}

			return (T)comp;
		}

#if UNITY_EDITOR
		/// <summary>
		/// Saves a screenshot to the specified path, or a default path
		/// under $ProjectBaseDir/Screenshots/
		/// </summary>
		public static void CaptureScreenshot (string fileName = null)
		{
			if (string.IsNullOrEmpty (fileName)) {
				fileName = GetDefaultScreenshotPath ();
			}
			
			Debug.Log("Saving screenshot to: " + fileName);
			Application.CaptureScreenshot(fileName);
		}

		private static string GetDefaultScreenshotPath ()
		{
			string fileName;
			int count = 0;

			// Create base directory if necessary.
			const string basePath = "Screenshots/";
			if (!System.IO.Directory.Exists(basePath)) {
				System.IO.Directory.CreateDirectory(basePath);
			}
			
			// Use the time as the file name.
			System.DateTime time = System.DateTime.Now;
			string timeStr = time.ToString("yyyy-MM-dd-HH-mm-ss-");
			
			do {
				fileName = basePath + timeStr + count + ".png";
				count++;
			} while (System.IO.File.Exists(fileName));

			return fileName;
		}
#endif // UNITY_EDITOR

#if UNITY_IPHONE
		/// <summary>
		/// Gets the iOS major version #.
		/// Example: iOS 7.0.1 returns 7.
		/// </summary>
		public static int GetMajorVersion_iOS ()
		{
			// String in the form of: iPhone OS 6.1
			string osString = SystemInfo.operatingSystem;
			
			string versionString = osString.Replace("iPhone OS ", "");
			string[] tmpStrings = versionString.Split ('.');
			
			if (tmpStrings.Length == 0) {
				Debug.LogWarning ("Invalid version string.");
				return -1;
			}
			
			string majorVersionString = tmpStrings [0];
			
			return System.Convert.ToInt32 (majorVersionString);
		}
#endif // UNITY_IPHONE
	}
}
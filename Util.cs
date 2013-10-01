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
	}
}
using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Allows easy access to all existing pools.
	/// </summary>
	public class GameObjectPoolMgr
	{
		private static Dictionary<string, GameObjectPool> _pools = new Dictionary<string, GameObjectPool> ();
		
		public static void RegisterPool (string poolName, GameObjectPool pool)
		{
			// Ignore empty string pools.
			if (string.IsNullOrEmpty (poolName)) {
				return;
			}
			
			if (_pools.ContainsKey (poolName)) {
				Debug.LogError ("Pool [" + poolName + "] already exists.");
				return;
			}
			
			_pools.Add (poolName, pool);
		}
		
		public static void UnregisterPool (string poolName)
		{
			_pools.Remove (poolName);
		}
		
		public static GameObjectPool GetPool (string poolName)
		{
			GameObjectPool pool;
			if (_pools.TryGetValue (poolName, out pool)) {
				return pool;
			}
			
			return null;
		}
	}
}
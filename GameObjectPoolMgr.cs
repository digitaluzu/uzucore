using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
	public class GameObjectPoolMgr : BaseBehaviour
	{
		/// <summary>
		/// Spawn an GameObject from a specified pool at a given position.
		/// </summary>
		static public GameObject Spawn (string poolName, Vector3 position)
		{
			GameObjectPool pool;
			if (Instance._pools.TryGetValue (poolName, out pool)) {
				return pool.Spawn (position);
			}
			
			Debug.LogError ("Pool [" + poolName + "] does not exist.");
			return null;
		}
		
		#region Implementation.
		#region Singleton implementation.
		private static GameObjectPoolMgr _instance;
	
		private static GameObjectPoolMgr Instance {
			get { return _instance; }
			set { Uzu.Util.SingletonSet<GameObjectPoolMgr> (ref _instance, value); }
		}
		#endregion
		
		private Dictionary<string, GameObjectPool> _pools = new Dictionary<string, GameObjectPool> ();
	
		[System.Serializable]
			//private class PoolEntry
		public class PoolEntry //Force to make this public to access form editor?
		{
			public PoolEntry (GameObject obj, int ncount)
			{
				name = obj.name;
				gameObject = obj;
				count = ncount;
			}

			public string name = string.Empty;
			public int count = 0;
			public GameObject gameObject = null;
		}
		
		/// <summary>
		/// Allows registration of pool names/counts from editor.
		/// Dictionary is not supported as a serializable field, so
		/// we have to do it this way D:
		/// </summary>
		[SerializeField]
		private List<PoolEntry> _poolEntries;
	
		protected override void Awake ()
		{
			Instance = this;
			
			CreatePools ();
		}
			
		private void CreatePools ()
		{
			for (int i = 0; i < _poolEntries.Count; ++i) {
				PoolEntry entry = _poolEntries [i];
				
				// Check for duplication.
				if (_pools.ContainsKey (entry.name)) {
					Debug.LogError ("Pool [" + "] is registered twice.");
					continue;
				}
				
				if (entry.gameObject == null) {
					Debug.LogError ("GameObject for pool [" + entry.name + "] is null!");
					continue;
				}
				
				// Create the pool.
				GameObjectPool pool = new GameObjectPool (entry.gameObject, entry.count);
				_pools.Add (entry.name, pool);
				
				// Add as child to this manager.
				pool.PoolParent.transform.parent = this.transform;
			}
		}
		
		private void OnDestroy ()
		{
			Instance = null;
		}
		#endregion
	}
}
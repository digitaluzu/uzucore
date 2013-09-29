using UnityEngine;
using System.Collections.Generic;

public class UzuGameObjectPoolMgr : UzuBehaviour
{
	/// <summary>
	/// Spawn an GameObject from a specified pool at a given position.
	/// </summary>
	static public GameObject Spawn (string poolName, Vector3 position)
	{
		UzuGameObjectPool pool;
		if (Instance._pools.TryGetValue (poolName, out pool)) {
			return pool.Spawn (position);
		}
		
		Debug.LogError ("Pool [" + poolName + "] does not exist.");
		return null;
	}
	
	#region Implementation.
	#region Singleton implementation.
	private static UzuGameObjectPoolMgr _instance;

	private static UzuGameObjectPoolMgr Instance {
		get { return _instance; }
		set { UzuUtil.SingletonSet<UzuGameObjectPoolMgr> (ref _instance, value); }
	}
	#endregion
	
	private Dictionary<string, UzuGameObjectPool> _pools = new Dictionary<string, UzuGameObjectPool> ();

	[System.Serializable]
	//private class PoolEntry
	public class PoolEntry //Force to make this public to access form editor?
	{
		public PoolEntry(GameObject obj, int ncount){
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
	//private List<PoolEntry> _poolEntries;
	public List<PoolEntry> _poolEntries; //Force to make this public to access form editor?

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
			_pools.Add (entry.name, new UzuGameObjectPool (entry.gameObject, entry.count));
		}
	}
	
	private void OnDestroy ()
	{
		Instance = null;
	}
	#endregion
}

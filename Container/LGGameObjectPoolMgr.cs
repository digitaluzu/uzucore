using UnityEngine;
using System.Collections.Generic;

public class LGGameObjectPoolMgr : LGBehaviour
{
	/// <summary>
	/// Spawn an GameObject from a specified pool at a given position.
	/// </summary>
	static public GameObject Spawn (string poolName, Vector3 position)
	{
		LGGameObjectPool pool;
		if (Instance._pools.TryGetValue (poolName, out pool)) {
			return pool.Spawn (position);
		}
		
		Debug.LogError ("Pool [" + poolName + "] does not exist.");
		return null;
	}
	
	/*
	static public void Unspawn (string poolName,GameObject obj )
	MEMO: The manager don't have an Unspawn method.
	To Unspawn an object derive your object from LGPooledBehaviour and use the Unspawn methode from LGPooledBehaviour 
	*/
	
	
	#region Implementation.
	#region Singleton implementation.
	private static LGGameObjectPoolMgr _instance;

	private static LGGameObjectPoolMgr Instance {
		get { return _instance; }
		set { LGUtil.SingletonSet<LGGameObjectPoolMgr> (ref _instance, value); }
	}
	#endregion
	
	private Dictionary<string, LGGameObjectPool> _pools = new Dictionary<string, LGGameObjectPool> ();

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
			_pools.Add (entry.name, new LGGameObjectPool (entry.gameObject, entry.count));
		}
	}
	
	private void OnDestroy ()
	{
		Instance = null;
	}
	#endregion
}

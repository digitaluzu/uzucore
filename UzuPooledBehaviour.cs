using UnityEngine;

/// <summary>
/// Base class for pooled objects.
/// Utilizes UzuGameObjectPoolMgr for spawning.
/// 
/// For pooled behaviours, OnEnable() should be used instead of Start()
/// for object initialization. Pooled behaviours are reused instead of
/// destroyed, so their Start() function is only called once whereas
/// OnEnable() is called every time a new object is spawned.
/// </summary>
public class UzuPooledBehaviour : UzuBehaviour
{
	#region Pooling.
	private UzuGameObjectPool _ownerPool;
	
	/// <summary>
	/// Called when this object is initially allocated and added to a pool.
	/// </summary>
	private void OnSpawned (UzuGameObjectPool pool)
	{
		if (_ownerPool != null) {
			Debug.LogError ("Entity already belongs to a pool!");
			return;
		}	
		_ownerPool = pool;
	}
	
	/// <summary>
	/// Call when this object is no longer needed and can be returned to the pool.
	/// </summary>
	public void Unspawn ()
	{
		OnUnspawn ();
		
		// Unspawn.
		_ownerPool.Unspawn (this.gameObject);
	}
	
	/// <summary>
	/// Called during Unspawn.
	/// Allow derived class to do any cleanup before unspawn occurs.
	/// </summary>
	protected virtual void OnUnspawn ()
	{	
	}
	#endregion
}
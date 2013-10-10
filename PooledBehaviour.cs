using UnityEngine;

namespace Uzu
{
	/// <summary>
	/// Base class for pooled objects.
	/// Utilizes GameObjectPoolMgr for spawning.
	/// 
	/// For pooled behaviours, OnEnable() should be used instead of Start()
	/// for object initialization. Pooled behaviours are reused instead of
	/// destroyed, so their Start() function is only called once whereas
	/// OnEnable() is called every time a new object is spawned.
	/// </summary>
	public class PooledBehaviour : BaseBehaviour
	{
		#region Pooling.
		private GameObjectPool _ownerPool;
		
		/// <summary>
		/// Called when this object is initially allocated and added to a pool.
		/// </summary>
		public void AddToPool (GameObjectPool pool)
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
			// Unspawn.
			if (_ownerPool != null) {
				_ownerPool.Unspawn (this.gameObject);
			}
		}
		
		/// <summary>
		/// Callback triggered when event is spawn.
		/// Allow derived class to do any initial setup.
		/// </summary>
		public virtual void OnSpawn ()
		{	
		}
		
		/// <summary>
		/// Calledback triggered by Unspawn.
		/// Allow derived class to do any cleanup before unspawn occurs.
		/// </summary>
		public virtual void OnUnspawn ()
		{	
		}
		#endregion
	}
}
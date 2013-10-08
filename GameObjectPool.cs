using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Class for pooling GameObjects.
	/// </summary>
	public class GameObjectPool : BaseBehaviour
	{
		public int ActiveObjectCount {
			get { return _allObjects.Count - _availableObjects.Count; }
		}
		
		public List<GameObject> ActiveObjects {
			get {
				List<GameObject> activeObjects = new List<GameObject> (ActiveObjectCount);
				for (int i = 0; i < _allObjects.Count; i++) {
					PooledBehaviour obj = _allObjects [i];
					if (!_availableObjects.Contains (obj)) {
						activeObjects.Add (obj.gameObject);
					}
				}
				return activeObjects;
			}
		}
			
		/// <summary>
		/// Spawn a new GameObject with the given position.
		/// Re-uses an object in the pool if available.
		/// </summary>
		public GameObject Spawn (Vector3 position)
		{
			return Spawn (position, _prefabTransform.localRotation);
		}
		
		/// <summary>
		/// Spawn a new GameObject with the given position/rotation.
		/// Re-uses an object in the pool if available.
		/// 
		/// Triggers message OnSpawned( GameObjectPool ) on instantiation
		/// of GameObject.
		/// </summary>
		public GameObject Spawn (Vector3 position, Quaternion rotation)
		{
			GameObject resultGO;
			PooledBehaviour resultComponent;
			
			if (_availableObjects.Count == 0) {
				// Should we automatically grow?
				if (!_doesGrow) {
					Debug.LogWarning ("Pool capacity [" + _initialCount + "] reached.");
					return null;
				}
				
				resultGO = CreateObject (position, rotation);
				resultComponent = resultGO.GetComponent<PooledBehaviour> ();
			} else {
				resultComponent = _availableObjects.Pop ();
				resultGO = resultComponent.gameObject;
				
				Transform resultTransform = resultComponent.CachedXform;
				resultTransform.localPosition = position;
				resultTransform.localRotation = rotation;
			}
			
			// Activate.
			resultGO.SetActive (true);
			resultComponent.OnSpawn ();
			
			return resultGO;
		}
		
		/// <summary>
		/// Unspawns a given GameObject and adds it back to the available
		/// resource pool.
		/// </summary>
		public void Unspawn (GameObject targetGO)
		{
			PooledBehaviour targetComponent = targetGO.GetComponent<PooledBehaviour> ();
			
	#if UNITY_EDITOR
			if (targetComponent == null || !_allObjects.Contains(targetComponent)) {
				Debug.LogError("Attempting to Unspawn an object not belonging to this pool!");
				return;
			}
	#endif // UNITY_EDITOR
			
			if (!_availableObjects.Contains (targetComponent)) {
				targetComponent.OnUnspawn ();
				targetGO.SetActive (false);
				_availableObjects.Push (targetComponent);
			}
		}
		
		/// <summary>
		/// Unspawns all GameObjects and adds them all back to the available
		/// resource pool.
		/// </summary>
		public void UnspawnAll ()
		{
			for (int i = 0; i < _allObjects.Count; i++) {
				Unspawn (_allObjects [i].gameObject);
			}
		}
		
		/// <summary>
		/// Destroys all GameObjects in the pool.
		/// </summary>
		public void DestroyAll ()
		{
			for (int i = 0; i < _allObjects.Count; i++) {
				GameObject.Destroy (_allObjects [i].gameObject);
			}
			
			_availableObjects.Clear ();
			_allObjects.Clear ();
		}
		
		#region Implementation.
		[SerializeField]
		private string _poolName;
		[SerializeField]
		private int _initialCount;
		[SerializeField]
		private GameObject _prefab;
		[SerializeField]
		private bool _doesGrow = true;
		private GameObject _poolParent;
		private Transform _prefabTransform;
		private Stack<PooledBehaviour> _availableObjects;
		private List<PooledBehaviour> _allObjects;
		
		private GameObject CreateObject (Vector3 position, Quaternion rotation)
		{
			GameObject resultGO = GameObject.Instantiate (_prefab, position, rotation) as GameObject;
			PooledBehaviour resultComponent = resultGO.GetComponent<PooledBehaviour> ();
			
			if (resultComponent == null) {
				Debug.LogError ("Pooled object must contain a Uzu.PooledBehaviour component.");
				GameObject.Destroy (resultGO);
				return null;
			}
			
			Transform resultTransform = resultComponent.CachedXform;
			resultTransform.parent = _poolParent.transform;
			resultTransform.localPosition = position;
			resultTransform.localRotation = rotation;
			resultTransform.localScale = _prefabTransform.localScale;
			
			_allObjects.Add (resultComponent);
			resultComponent.AddToPool (this);
			
			return resultGO;
		}
		
		protected override void Awake ()
		{
			base.Awake ();
			
			// Create a parent for grouping all objects together.
			{
				StringBuilder stringBuilder = new StringBuilder ("UzuPool - ");
				stringBuilder.Append (_prefab.name);
				_poolParent = new GameObject (stringBuilder.ToString ());
				
				Transform parentXform = _poolParent.transform;
				parentXform.parent = CachedXform;
				parentXform.localScale = Vector3.one;
			}
			
			_prefabTransform = _prefab.transform;			
			_availableObjects = new Stack<PooledBehaviour> (_initialCount);
			_allObjects = new List<PooledBehaviour> (_initialCount);
			
			// Pre-allocate our pool.
			{
				// Allocate objects.
				for (int i = 0; i < _initialCount; ++i) {
					CreateObject (Vector3.zero, Quaternion.identity);
				}
				
				// Add to pool.
				for (int i = 0; i < _allObjects.Count; i++) {
					GameObject targetGO = _allObjects [i].gameObject;
					targetGO.SetActive (false);
					
					PooledBehaviour targetComponent = targetGO.GetComponent<PooledBehaviour> ();
					_availableObjects.Push (targetComponent);
				}
			}
			
			// Register.
			GameObjectPoolMgr.RegisterPool (_poolName, this);
		}
		
		private void OnDestroy ()
		{
			// Unregister.
			GameObjectPoolMgr.UnregisterPool (_poolName);
		}
		#endregion
	}
}
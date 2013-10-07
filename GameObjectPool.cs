using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Class for pooling GameObjects.
	/// 
	/// Based loosely on the description here:
	/// http://vonlehecreative.wordpress.com/2010/01/06/unity-resource-gameobjectpool/
	/// </summary>
	public class GameObjectPool
	{
		private GameObject _poolParent;
		private GameObject _prefab;
		private Transform _prefabTransform;
		private Stack<PooledBehaviour> _availableObjects;
		private List<PooledBehaviour> _allObjects;
		
		public int ActiveObjectCount {
			get { return _allObjects.Count - _availableObjects.Count; }
		}
		
		public GameObject PoolParent {
			get { return _poolParent; }
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
		
		public GameObjectPool (GameObject prefab, int initialCapacity)
		{
			// Create a parent for group all objects together.
			{
				StringBuilder stringBuilder = new StringBuilder ("UzuPool - ");
				stringBuilder.Append (prefab.name);
				_poolParent = new GameObject (stringBuilder.ToString ());
			}
			_prefab = prefab;
			_prefabTransform = _prefab.transform;
			
			_availableObjects = new Stack<PooledBehaviour> (initialCapacity);
			_allObjects = new List<PooledBehaviour> (initialCapacity);
			
			// Pre-allocate our objects.
			for (int i = 0; i < initialCapacity; ++i) {
				Spawn (Vector3.zero);
			}
			UnspawnAll ();
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
				resultGO = GameObject.Instantiate (_prefab, position, rotation) as GameObject;
				resultComponent = resultGO.GetComponent<PooledBehaviour> ();
				
				if (resultComponent == null) {
					Debug.LogError ("Pooled object must contain a Uzu.PooledBehaviour component.");
					return resultGO;
				}
				
				Transform resultTransform = resultComponent.CachedXform;
				resultTransform.parent = _poolParent.transform;
				resultTransform.localPosition = position;
				resultTransform.localRotation = rotation;
				resultTransform.localScale = _prefabTransform.localScale;
				
				_allObjects.Add (resultComponent);
				resultComponent.AddToPool (this);
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
	}
}
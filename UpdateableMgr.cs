using UnityEngine;
using System;
using System.Collections.Generic;

namespace Uzu
{	
	/// <summary>
	/// Provides basic update functionality for registered objects.
	/// </summary>
	public class UpdateableMgr : BaseBehaviour
	{	
		/// <summary>
		/// Registers an object to be updated every frame.
		/// </summary>
		static public void RegisterForUpdates (UpdateableInterface updateable)
		{
			UpdateableMgr instance = Instance;
			
#if DEBUG
			// Duplicate check.
			foreach (WeakReference weakRef in instance._updateables) {
				UpdateableInterface u = weakRef.Target as UpdateableInterface;
				if (u == updateable) {
					Debug.LogWarning ("UzuUpdateableInterface[" + updateable + "] already registered.");
					return;
				}
			}
#endif
			
			instance._updateables.Add (new WeakReference (updateable));
		}
		
		#region Implementation.
		#region Singleton implementation.
		private static UpdateableMgr _instance;
	
		private static UpdateableMgr Instance {
			get {
				// Create on demand.
				if (_instance == null) {
					GameObject go = new GameObject ("UzuUpdater");
					_instance = go.AddComponent<UpdateableMgr> ();
				}
				return _instance;
			}
			set { Uzu.Util.SingletonSet<UpdateableMgr> (ref _instance, value); }
		}
		#endregion
		
		/// <summary>
		/// List of all the updateable objects.
		/// We use weak references so that updateable objects don't need
		/// to worry about unregistering.
		/// </summary>
		private List<WeakReference> _updateables = new List<WeakReference> ();
		
		protected override void Awake ()
		{
			base.Awake ();
			
			Instance = this;
			DontDestroyOnLoad (gameObject);
		}
		
		protected void Update ()
		{
			// Update events.
			for (int i = 0; i < _updateables.Count; /*++i*/) {
				UpdateableInterface u = _updateables [i].Target as UpdateableInterface;
				if (u == null) {
					_updateables.RemoveAt (i);
				} else {
					u.OnUpdate ();
					i++;
				}
			}
		}
		#endregion
	}
}
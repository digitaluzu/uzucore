using UnityEngine;
using System.Collections;

public class UzuGameStateMgr : UzuBehaviour
{
	/// <summary>
	/// Get the current state.
	/// </summary>
	public static UzuGameStateInterface CurrentState {
		get { return Instance._currentState; }
	}
	
	/// <summary>
	/// Get the previous state.
	/// </summary>
	public static UzuGameStateInterface PreviousState {
		get { return Instance._previousState; }
	}

	/// <summary>
	/// Change state immediately.
	/// </summary>
	public static void ChangeState (UzuGameStateInterface newState)
	{
		if (newState == null) {
			Debug.LogError ("Invalid state (null).");
			return;
		}
		
		UzuGameStateMgr instance = Instance;
		if (newState == instance._currentState) {
			Debug.LogWarning ("Already in state [" + newState.StateName + "].");
			return;
		}
		
		if (instance._currentState != null) {
			Debug.Log("Exiting: " + instance._currentState.StateName);
			instance._currentState.OnExit ();
		}
		
		instance._previousState = instance._currentState;
		instance._currentState = newState;
		
		Debug.Log("Entering: " + instance._currentState.StateName);
		instance._currentState.OnEnter ();
	}

	#region Implementation.
	#region Singleton implementation.
	private static UzuGameStateMgr _instance;

	private static UzuGameStateMgr Instance {
		get {
			// Create on demand.
			if (_instance == null) {
				GameObject go = new GameObject ("UzuGameStateMgr");
				_instance = go.AddComponent<UzuGameStateMgr> ();
			}
			return _instance;
		}
		set { LGUtil.SingletonSet<UzuGameStateMgr> (ref _instance, value); }
	}
	#endregion
	
	private UzuGameStateInterface _previousState;
	private UzuGameStateInterface _currentState;

	protected override void Awake ()
	{
		base.Awake();
		
		// Only allow single instance to exist.
		if (_instance != null && _instance != this) {
			Destroy (gameObject);
			return;
		}
		
		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	protected void Update ()
	{
		// Update the current state.
		if (_currentState != null) {
			_currentState.OnUpdate ();
		}
	}
	
	protected void OnDestroy ()
	{
		Instance = null;
	}
	#endregion
}

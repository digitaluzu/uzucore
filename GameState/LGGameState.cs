using UnityEngine;

/// <summary>
/// Interface for a single game state.
/// </summary>
public interface LGIGameState
{
	string StateName { get; }
	
	/// <summary>
	/// Called when the state is activated.
	/// </summary>
	void OnEnter ();
	
	/// <summary>
	/// Called every frame while the state is active.
	/// </summary>
	void OnUpdate ();
	
	/// <summary>
	/// Called right before the state is deactivated.
	/// </summary>
	void OnExit ();
}

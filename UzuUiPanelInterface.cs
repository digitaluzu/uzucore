using UnityEngine;

/// <summary>
/// Base interface for all panel types.
/// </summary>
public interface UzuUiPanelInterface
{
	/// <summary>
	/// Gets the name of this panel.
	/// </summary>
	string GetName ();
	
	/// <summary>
	/// Initialize the panel.
	/// </summary>
	/// <param name='ownerManager'>
	/// Owner manager.
	/// </param>
	void Initialize (UzuUiPanelMgr ownerManager);
	
	/// <summary>
	/// Activate this instance.
	/// </summary>
	void Activate ();
	
	/// <summary>
	/// Deactivate this instance.
	/// </summary>
	void Deactivate ();
	
	/// <summary>
	/// Called every frame while this panel is active.
	/// </summary>
	void DoUpdate ();
}
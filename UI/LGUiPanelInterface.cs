using UnityEngine;

/// <summary>
/// Base interface for all panel types.
/// </summary>
public interface LGUIPanelInterface
{
	/// <summary>
	/// Initialize the panel.
	/// </summary>
	/// <param name='ownerManager'>
	/// Owner manager.
	/// </param>
	void Initialize (LGUiPanelManager ownerManager);
	
	/// <summary>
	/// Activate this instance.
	/// </summary>
	void Activate ();
	
	/// <summary>
	/// Deactivate this instance.
	/// </summary>
	void Deactivate ();
}
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Uzu user interface panel manager.
/// 
/// Usage:
///  1) Add UzuUiPanelMgr to the scene.
///  2) Add all panels as children.
/// 
/// Note:
///  Multiple UzuUiPanelMgrs can exist in the scene at once.
/// </summary>
public class UzuUiPanelMgr : UzuBehaviour
{
	/// <summary>
	/// Changes the current panel to the specified panel id.
	/// </summary>
	public void ChangeCurrentPanel (string panelId)
	{
		UzuUiPanelInterface panel;
		if (_uiPanelDataHolder.TryGetValue (panelId, out panel)) {
			if (_currentPanel != null) {
				_currentPanel.Deactivate ();
			}
			_currentPanel = panel;
			_currentPanelId = panelId;
			_currentPanel.Activate ();
		} else {
			Debug.LogError ("Unable to activate a panel that is not registered: " + panelId);
		}
	}
	
	/// <summary>
	/// Gets the currently active panel id.
	/// </summary>
	public string CurrentPanelId {
		get { return _currentPanelId; }
	}
	
	/// <summary>
	/// Gets the currently active panel.
	/// </summary>
	public UzuUiPanelInterface CurrentPanel {
		get { return _currentPanel; }
	}

	#region Implementation.
	private Dictionary<string, UzuUiPanelInterface> _uiPanelDataHolder = new Dictionary<string, UzuUiPanelInterface> ();
	private UzuUiPanelInterface _currentPanel;
	private string _currentPanelId;
	
	private void RegisterPanel (string name, UzuUiPanelInterface panel)
	{
		_uiPanelDataHolder [name] = panel;

		// Initialize the panel.
		panel.Initialize (this);
	}
	
	protected override void Awake ()
	{
		base.Awake ();

		//Connect all the child panel to this panelManager
		UzuUiPanel[] managedPanelList = (UzuUiPanel[])this.gameObject.GetComponentsInChildren<UzuUiPanel> (true);		
		//Register all panel
		foreach (UzuUiPanel panel in managedPanelList) {
			RegisterPanel (panel.gameObject.name, panel);
		}
	}
	
	private void Update ()
	{
		// Update the current panel.
		if (_currentPanel != null) {
			_currentPanel.DoUpdate ();
		}
	}
	#endregion
}

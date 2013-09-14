using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// LG user interface panel manager.
/// Usage:
///  1) Add LGUiPannelManager to the scene.
///  2) Add all panels as children to the panel manager.
/// 
/// Note: 
///  You can have more than one PanelManger in one scene if needed.
///     (this is a rare case but this alow to have more that one panel active in same time)
/// </summary>
public class LGUiPanelManager : LGBehaviour
{
	/// <summary>
	/// Changes the current panel to the specified panel id.
	/// </summary>
	public void ChangeCurrentPanel (string panelId)
	{
		LGUIPanelInterface panel;
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
	public LGUIPanelInterface CurrentPanel {
		get { return _currentPanel; }
	}

	#region Implementation.
	private Dictionary<string, LGUIPanelInterface> _uiPanelDataHolder = new Dictionary<string, LGUIPanelInterface> ();
	private LGUIPanelInterface _currentPanel;
	private string _currentPanelId;
	
	private void RegisterPanel (string name, LGUIPanelInterface panel)
	{
//		Debug.Log(name);
		_uiPanelDataHolder [name] = panel;

		// Initialize the panel.
		panel.Initialize (this);
	}
	
	protected override void Awake ()
	{
		base.Awake ();

		//Connect all the child panel to this panelManager
		LGUiPanel[] managedPanelList = (LGUiPanel[])this.gameObject.GetComponentsInChildren<LGUiPanel> (true);		
		//Register all panel
		foreach (LGUiPanel panel in managedPanelList) {
			RegisterPanel (panel.gameObject.name, panel);
		}
		
		//Connect all the child panel to this panelManager
		LGUiLwfPanel[] lwfPanelList = (LGUiLwfPanel[])this.gameObject.GetComponentsInChildren<LGUiLwfPanel> (true);		
		//Register all panel
		foreach (LGUiLwfPanel panel in lwfPanelList) {
			RegisterPanel (panel.gameObject.name, panel);
		}
	}
	#endregion
}

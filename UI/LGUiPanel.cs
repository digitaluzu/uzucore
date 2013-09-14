using UnityEngine;
using System.Collections;

/// <summary>
/// LG user interface panel.
/// 
/// How to:
/// Override this class to create your custom panel Logic
/// </summary>
public class LGUiPanel : LGBehaviour , LGUIPanelInterface
{
	#region Implementation	
	/// <summary>
	/// The _owner manager of this panel.
	/// </summary>
	private LGUiPanelManager _ownerManager;
	
	/// <summary>
	/// Gets or sets the owner manager.
	/// </summary>
	protected LGUiPanelManager OwnerManager {
		get {
			return _ownerManager;
		}
		private set {
			_ownerManager = value;
		}
	}
	
	#region LGUIPanelInterface implementation.
	/// <summary>
	/// Initialize this panel.
	/// Create the link between the panel manager and the 
	/// </summary>
	public void Initialize (LGUiPanelManager ownerManager)
	{
		_ownerManager = ownerManager;	
		
		//Connect all the child widget to this panel
		Component[] widgets = this.gameObject.GetComponentsInChildren<LGUiWidget> (true);
		foreach (LGUiWidget widget in widgets) {
			widget.Initialize (this);
		}
		
		OnInitialize();
		
		// Deactivate the object just in case it was active during edit mode.
		// We don't want to call Deactivate, since this triggers the callback.
		this.gameObject.SetActive (false);
	}
	
	/// <summary>
	/// Activate this instance.
	/// </summary>
	public void Activate ()
	{
		this.gameObject.SetActive (true);
		
		OnActivate();
	}
	
	/// <summary>
	/// Deactivate this instance.
	/// </summary>
	public void Deactivate ()
	{
		this.gameObject.SetActive (false);
		
		OnDeactivate();
	}
	#endregion
	#endregion
	
	#region Events.
	/// <summary>
	/// Called when the panel is first initialized.
	/// </summary>
	public virtual void OnInitialize ()
	{	
	}
	
	/// <summary>
	/// Called when the panel is activated.
	/// </summary>
	public virtual void OnActivate()
	{	
	}
	
	/// <summary>
	/// Called when the panel is deactivated.
	/// </summary>
	public virtual void OnDeactivate()
	{
	}

	public virtual void OnHover (LGUiWidget widget, bool isOver)
	{
	}

	public virtual void OnPress (LGUiWidget widget, bool isPressed)
	{
	}

	public virtual void OnClick (LGUiWidget widget)
	{
	}

	public virtual void OnDoubleClick (LGUiWidget widget)
	{
	}

	public virtual void OnSelect (LGUiWidget widget, bool selected)
	{
	}

	public virtual void OnDrag (LGUiWidget widget, Vector2 delta)
	{
	}

	public virtual void OnDrop (LGUiWidget widget, GameObject go)
	{
	}

	public virtual void OnInput (LGUiWidget widget, string text)
	{
	}

	public virtual void OnSubmit (LGUiWidget widget)
	{
	}

	public virtual void OnScroll (LGUiWidget widget, float delta)
	{
	}
	#endregion
}

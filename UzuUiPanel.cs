using UnityEngine;
using System.Collections;

/// <summary>
/// Uzu user interface panel.
/// 
/// How to:
/// Override this class to create your custom panel Logic
/// </summary>
public class UzuUiPanel : UzuBehaviour, UzuUiPanelInterface
{
	#region Implementation	
	/// <summary>
	/// The _owner manager of this panel.
	/// </summary>
	private UzuUiPanelMgr _ownerManager;
	
	/// <summary>
	/// Gets or sets the owner manager.
	/// </summary>
	protected UzuUiPanelMgr OwnerManager {
		get {
			return _ownerManager;
		}
		private set {
			_ownerManager = value;
		}
	}
	
	#region UzuUiPanelInterface implementation.
	/// <summary>
	/// Initialize this panel.
	/// Create the link between the panel manager and the 
	/// </summary>
	public void Initialize (UzuUiPanelMgr ownerManager)
	{
		_ownerManager = ownerManager;	
		
		//Connect all the child widget to this panel
		Component[] widgets = this.gameObject.GetComponentsInChildren<UzuUiWidget> (true);
		foreach (UzuUiWidget widget in widgets) {
			widget.Initialize (this);
		}
		
		OnInitialize ();
		
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
		
		OnActivate ();
	}
	
	/// <summary>
	/// Deactivate this instance.
	/// </summary>
	public void Deactivate ()
	{
		this.gameObject.SetActive (false);
		
		OnDeactivate ();
	}
	
	public void DoUpdate ()
	{
		OnUpdate ();
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
	public virtual void OnActivate ()
	{	
	}
	
	/// <summary>
	/// Called when the panel is deactivated.
	/// </summary>
	public virtual void OnDeactivate ()
	{
	}
	
	/// <summary>
	/// Called every frame that this panel is active.
	/// </summary>
	public virtual void OnUpdate ()
	{		
	}

	public virtual void OnHover (UzuUiWidget widget, bool isOver)
	{
	}

	public virtual void OnPress (UzuUiWidget widget, bool isPressed)
	{
	}

	public virtual void OnClick (UzuUiWidget widget)
	{
	}

	public virtual void OnDoubleClick (UzuUiWidget widget)
	{
	}

	public virtual void OnSelect (UzuUiWidget widget, bool selected)
	{
	}

	public virtual void OnDrag (UzuUiWidget widget, Vector2 delta)
	{
	}

	public virtual void OnDrop (UzuUiWidget widget, GameObject go)
	{
	}

	public virtual void OnInput (UzuUiWidget widget, string text)
	{
	}

	public virtual void OnSubmit (UzuUiWidget widget)
	{
	}

	public virtual void OnScroll (UzuUiWidget widget, float delta)
	{
	}
	#endregion
}

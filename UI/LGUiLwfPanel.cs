using UnityEngine;
using System.Collections;
using System;

public class LGUiLwfPanel : LWFObject, LGUIPanelInterface
{	
	#region Implementation.	
	/// <summary>
	/// The _owner manager of this panel.
	/// </summary>
	private LGUiPanelManager _ownerManager;
	
	/// <summary>
	/// The _camera using the render the flash GUI
	/// </summary>
	protected  Camera _camera; //TODO I could change this to outomaticly set the camera to the parent UI camera.
	
	/// <summary>
	/// Gets or sets the owner manager.
	/// </summary>
	/// <value>
	/// The owner manager.
	/// </value>
	public LGUiPanelManager OwnerManager {
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
	/// <param name='thisPannelmanager'>
	/// This pannelmanager.
	/// </param>
	public void Initialize (LGUiPanelManager ownerManager)
	{
		_ownerManager = ownerManager;
		
		SetTargetCamera ();
		
		OnInitialize ();
		
		// Deactivate the object just in case it was active during edit mode.
		// We don't want to call Deactivate, since this triggers the callback.
		this.gameObject.SetActive (false);
	}
	
	/// <summary>
	/// Activate this instance.
	/// </summary>
	public override void Activate ()
	{
		base.Activate ();
		this.gameObject.SetActive (true);
		OnActivate ();
	}
	
	/// <summary>
	/// Deactivate this instance.
	/// </summary>
	public override void Deactivate ()
	{
		base.Deactivate ();
		this.gameObject.SetActive (false);
		OnDeactivate ();
	}
	#endregion
	
	private void SetTargetCamera ()
	{
		Transform target = null;
		Transform current = transform;
		while (current.parent) {
			current = current.parent;
			if (current.tag == "UiCamera") {
				target = current;
				break;
			}
		}
		if (target) {
			_camera = (Camera)target.gameObject.GetComponent<Camera> ();
		} else {
			Debug.LogError ("Could not find the UI camera. You  need to have a camera Taget UICamera in the hierarchy");
		}
		
	}
	#endregion
	
	/// <summary>
	/// Registers the event handler. (You can't call this in Awake) 
	/// </summary>
	protected void RegisterEventHandler (string eventName, Action<LWF.Movie, LWF.Button> callBack)
	{
		lwf.SetEventHandler (eventName, callBack);
	}
	
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
	#endregion
}

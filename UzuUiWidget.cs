using UnityEngine;

/// <summary>
/// Uzu user interface widget.
/// </summary>
public class UzuUiWidget : UzuBehaviour
{	
	/// <summary>
	/// The _owner panel of this widget.
	/// </summary>
	private UzuUiPanel _ownerPanel;
	
	/// <summary>
	/// Initialize this widget and set the connection to the owner panel.
	/// </summary>
	public void Initialize (UzuUiPanel ownerPanel)
	{
		_ownerPanel = ownerPanel;
	}
	
	#region Events.
	private void OnHover (bool isOver)
	{
		_ownerPanel.OnHover (this, isOver);
	}

	private void OnPress (bool pressed)
	{
		_ownerPanel.OnPress (this, pressed);
	}
	
	private void OnClick ()
	{
		_ownerPanel.OnClick (this);
	}

	private void OnDoubleClick ()
	{
		_ownerPanel.OnDoubleClick (this);
	}

	private void OnSelect (bool selected)
	{
		_ownerPanel.OnSelect (this, selected);
	}

	private void OnDrag (Vector2 delta)
	{
		_ownerPanel.OnDrag (this, delta);
	}

	private void OnDrop (GameObject go)
	{
		_ownerPanel.OnDrop (this, go);
	}

	private void OnInput (string text)
	{
		_ownerPanel.OnInput (this, text);
	}

	private void OnSubmit ()
	{
		_ownerPanel.OnSubmit (this);
	}

	private void OnScroll (float delta)
	{
		_ownerPanel.OnScroll (this, delta);
	}
	#endregion
}
